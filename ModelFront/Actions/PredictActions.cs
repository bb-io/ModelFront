using Apps.ModelFront.Api;
using Apps.ModelFront.Constants;
using Apps.ModelFront.Extensions;
using Apps.ModelFront.Invocables;
using Apps.ModelFront.Models.Request.Predict;
using Apps.ModelFront.Models.Response.Predict;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.ModelFront.Actions;

[ActionList]
public class PredictActions : ModelFrontInvocable
{
    public PredictActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Predict", Description = "Get prediction data for a single segment")]
    public async Task<ResponseRow> Predict(
        [ActionParameter] PredictQuery query,
        [ActionParameter] RowRequest input)
    {
        var endpoint = ApiEndpoints.Predict.WithQuery(query);
        var request = new ModelFrontRequest(endpoint, Method.Post, Creds)
            .WithJsonBody(new PredictRequest
            {
                Rows = new List<RowRequest> { input }
            }, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<PredictResponse>(request);
        return response.Rows.First();
    }

    [Action("Predict multiple", Description = "Get prediction data for multiple segments")]
    public async Task<PredictResponse> PredictMany(
        [ActionParameter] PredictQuery query,
        [ActionParameter] PredictManyInput input)
    {
        var endpoint = ApiEndpoints.Predict.WithQuery(query);
        var chunks = input.Segments.ChunkBy(30);

        var result = new List<ResponseRow>();
        foreach (var chunk in chunks)
        {
            var request = new ModelFrontRequest(endpoint, Method.Post, Creds);
            request.WithJsonBody(new PredictRequest
            {
                Rows = chunk
            }, JsonConfig.Settings);

            var response = await Client.ExecuteWithErrorHandling<PredictResponse>(request);
            result.AddRange(response.Rows);
        }

        return new(result);
    }
}