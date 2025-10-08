using Apps.ModelFront.Utils;
using Apps.ModelFront.Models.Dto;
using Apps.ModelFront.Invocables;
using Apps.ModelFront.Models.Request.Predict;
using Apps.ModelFront.Models.Request.Review;
using Apps.ModelFront.Models.Response.Review;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Extensions;
using Blackbird.Filters.Transformations;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.ModelFront.Actions;

//Hidden from Blackbird UI until further tests
//[ActionList("Review")]
public class ReviewActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : ModelFrontInvocable(invocationContext)
{
    //[Action("Estimate quality (experimental)", Description = "Evaluate translation quality for interoperable files using ModelFront.")]
    public async Task<ScoreResponse> EstimateQuality([ActionParameter] ScoreRequest input)
    {
        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        Transformation transformation;

        try
        {
            transformation = await Transformation.Parse(fileStream, input.File.Name);
        }
        catch (Exception)
        {
            throw new PluginApplicationException("The provided file is not supported. Supported formats: XLIFF, HTML, plain text.");
        }

        var statesToEstimate = input
            .EstimateUnitsWhereAllSegmentStates?
            .Select(SegmentStateHelper.ToSegmentState)
            .Where(s => s != null)
            .Select(s => s!.Value)
            .ToList() ?? [SegmentState.Initial, SegmentState.Translated];

        var unitsToEstimate = transformation
            .GetUnits()
            .Where(u => u.Id != null)
            .Where(ReviewUtils.HasTranslatedContent)
            .Where(u => ReviewUtils.HasSegmentWithState(u, statesToEstimate))
            .ToList();

        if (unitsToEstimate.Count == 0)
        {
            return new ScoreResponse
            {
                File = input.File,
                TotalUnits = transformation.GetUnits().Count(),
                TotalUnitsProcessed = 0,
                TotalUnitsUnderThreshhold = 0,
                TotalSegmentsFinalized = 0,
                AverageScore = 0,
                PercentageUnitsUnderThreshold = 0,
                Usage = new UsageDto()
            };
        }

        var batches = unitsToEstimate.Batch(input.BucketSize ?? 1500);
        var newSegmentState = SegmentStateHelper.ToSegmentState(input.NewState ?? string.Empty) ?? SegmentState.Reviewed;

        var totalUnitsProcessed = 0;
        var totalUnitsUnderThreshold = 0;
        var totalSegmentsFinalized = 0;
        double totalScore = 0.0;

        var predictActions = new PredictActions(InvocationContext);

        foreach (var batch in batches)
        {
            var batchList = batch.ToList();

            var rows = batchList.Select(u => new RowRequest
            {
                Original = ReviewUtils.GetUnitSource(u.Unit),
                Translation = ReviewUtils.GetUnitTarget(u.Unit)
            }).ToList();

            var response = await predictActions.PredictMany(new PredictQuery(), new PredictManyInput { Segments = rows });

            for (int i = 0; i < batchList.Count; i++)
            {
                var (unit, segment) = batchList[i];
                var r = response.Rows[i];

                unit.Quality.Score = r.Quality;

                if (input.Threshold != null && r.Quality < input.Threshold)
                    totalUnitsUnderThreshold++;

                if (r.Quality >= input.Threshold)
                {
                    foreach (var seg in unit.Segments)
                    {
                        if (!statesToEstimate.Contains(seg.State ?? SegmentState.Initial))
                            continue;

                        seg.State = newSegmentState;
                        totalSegmentsFinalized++;
                    }
                }

                totalUnitsProcessed++;
                totalScore += r.Quality;
            }
        }

        var xliffStream = transformation.Serialize().ToStream();
        var uploadedFile = await fileManagementClient.UploadAsync(xliffStream, "application/xliff+xml", input.File.Name);

        return new ScoreResponse
        {
            File = uploadedFile,
            TotalUnits = transformation.GetUnits().Count(),
            TotalUnitsProcessed = totalUnitsProcessed,
            TotalUnitsUnderThreshhold = totalUnitsUnderThreshold,
            TotalSegmentsFinalized = totalSegmentsFinalized,
            AverageScore = totalUnitsProcessed > 0 ? totalScore / totalUnitsProcessed : 0,
            PercentageUnitsUnderThreshold = totalUnitsProcessed > 0
                ? (double)totalUnitsUnderThreshold / totalUnitsProcessed * 100
                : 0,
            Usage = new UsageDto()
        };
    }
}