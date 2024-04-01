using Apps.ModelFront.Invocables;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.ModelFront.Api;
using Apps.ModelFront.Constants;
using Apps.ModelFront.Models.Request;
using Apps.ModelFront.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using RestSharp;
using System.Xml.Linq;
using Apps.ModelFront.Models;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Apps.ModelFront.Constants;
using Apps.ModelFront.Models.Response.Predict;
using Apps.ModelFront.Models.Request.Predict;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;

namespace Apps.ModelFront.Actions;

[ActionList]
public class XliffActions : ModelFrontInvocable
{
    private readonly IFileManagementClient _fileManagementClient;
    public XliffActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
       invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Predict XLIFF", Description = "Get prediction data for an XLIFF 1.2 file")]
    public async Task<PredictXliffResponse> PredictXliff([ActionParameter] PredictQuery query,
        [ActionParameter] PredictXliffInput Input)
    {
        var _file = await _fileManagementClient.DownloadAsync(Input.File);

        var transunits = ExtractSegmentsFromXliff(_file);

        var results = new Dictionary<string, ResponseRow>();

        var file = await _fileManagementClient.DownloadAsync(Input.File);
        string fileContent;
        Encoding encoding;
        using (var inFileStream = new StreamReader(file, true))
        {
            encoding = inFileStream.CurrentEncoding;
            fileContent = inFileStream.ReadToEnd();
        }

        foreach (var transunit in transunits)
        {
            var endpoint = ApiEndpoints.Predict.WithQuery(query);
            var request = new ModelFrontRequest(endpoint, Method.Post, Creds)
                .WithJsonBody(new PredictRequest
                {
                    Rows = new List<RowRequest> { new RowRequest 
                    {
                        Original = transunit.Source,
                        Translation = transunit.Target
                    
                    } }
                }, JsonConfig.Settings);

            var response = await Client.ExecuteWithErrorHandling<PredictResponse>(request);
            results.Add(transunit.ID, response.Rows.First());

            fileContent = Regex.Replace(fileContent, @"(<trans-unit id=""" + transunit.ID + @""")", @"${1} extradata=""" + response.Rows.First().Quality + @"""");

        }

        if (Input.Threshold != null && Input.Condition != null && Input.State != null)
        {
            var filteredTUs = new List<string>();
            switch (Input.Condition)
            {
                case ">":
                    filteredTUs = results.Where(x => x.Value.Quality > Input.Threshold).Select(x => x.Key).ToList();
                    break;
                case ">=":
                    filteredTUs = results.Where(x => x.Value.Quality >= Input.Threshold).Select(x => x.Key).ToList();
                    break;
                case "=":
                    filteredTUs = results.Where(x => x.Value.Quality == Input.Threshold).Select(x => x.Key).ToList();
                    break;
                case "<":
                    filteredTUs = results.Where(x => x.Value.Quality < Input.Threshold).Select(x => x.Key).ToList();
                    break;
                case "<=":
                    filteredTUs = results.Where(x => x.Value.Quality <= Input.Threshold).Select(x => x.Key).ToList();
                    break;
            }

            fileContent = UpdateTargetState(fileContent, Input.State, filteredTUs);
        }

        return new PredictXliffResponse
        {
            AverageQuality = results.Values.Average(x => x.Quality),
            AverageRisk = results.Values.Average(x => x.Risk),
            File = await _fileManagementClient.UploadAsync(new MemoryStream(encoding.GetBytes(fileContent)), MediaTypeNames.Text.Xml, Input.File.Name)
        };
    }
    public List<TranslationUnit> ExtractSegmentsFromXliff(Stream inputStream)
    {
        var TUs = new List<TranslationUnit>();
        using var reader = new StreamReader(inputStream, Encoding.UTF8);
        var xliffDocument = XDocument.Load(reader);

        XNamespace defaultNs = xliffDocument.Root.GetDefaultNamespace();

        foreach (var transUnit in xliffDocument.Descendants(defaultNs + "trans-unit"))
        {
            TUs.Add(new TranslationUnit
            {
                ID = transUnit.Attribute("id")?.Value,
                Source = transUnit.Element(defaultNs + "source").Value,
                Target = transUnit.Element(defaultNs + "target").Value
            });
        }
        return TUs;
    }
    private string UpdateTargetState(string fileContent, string state, List<string> filteredTUs)
    {
        var tus = Regex.Matches(fileContent, @"<trans-unit[\s\S]+?</trans-unit>").Select(x => x.Value);
        foreach (var tu in tus.Where(x => filteredTUs.Any(y => y == Regex.Match(x, @"<trans-unit id=""(\d+)""").Groups[1].Value)))
        {
            string transformedTU = Regex.IsMatch(tu, @"<target(.*?)state=""(.*?)""(.*?)>") ?
                Regex.Replace(tu, @"<target(.*?state="")(.*?)("".*?)>", @"<target${1}" + state + "${3}>")
                : Regex.Replace(tu, "<target", @"<target state=""" + state + @"""");
            fileContent = Regex.Replace(fileContent, Regex.Escape(tu), transformedTU);
        }
        return fileContent;
    }

}





