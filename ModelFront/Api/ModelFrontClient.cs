using Apps.ModelFront.Constants;
using Apps.ModelFront.Models;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.ModelFront.Api;

public class ModelFrontClient : BlackBirdRestClient
{
    public ModelFrontClient() : base(new RestClientOptions
    {
        BaseUrl = Urls.Api.ToUri()
    })
    {
    }

    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        string content = (await ExecuteWithErrorHandling(request)).Content;
        T val = JsonConvert.DeserializeObject<T>(content, JsonSettings);
        if (val == null)
        {
            throw new Exception($"Could not parse {content} to {typeof(T)}");
        }

        return val;
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        RestResponse restResponse = await ExecuteAsync(request);
        if (!restResponse.IsSuccessStatusCode)
        {
            throw ConfigureErrorException(restResponse);
        }

        return restResponse;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var error = JsonConvert.DeserializeObject<Error>(response.Content);
        throw new PluginApplicationException(error.Message);
    }
}