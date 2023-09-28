using Apps.ModelFront.Api;
using Apps.ModelFront.Constants;
using Apps.ModelFront.Models.Response.Predict;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.ModelFront.Connections;

public class ConnectionValidator : IConnectionValidator
{
    protected ModelFrontClient Client => new();
    
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        var request = new ModelFrontRequest(ApiEndpoints.Models, Method.Get, authProviders);

        try
        {
            await Client.ExecuteWithErrorHandling(request);

            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}