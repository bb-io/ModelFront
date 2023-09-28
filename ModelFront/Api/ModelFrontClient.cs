using Apps.ModelFront.Constants;
using Apps.ModelFront.Models;
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

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var error = JsonConvert.DeserializeObject<Error>(response.Content);
        return new(error.Message);
    }
}