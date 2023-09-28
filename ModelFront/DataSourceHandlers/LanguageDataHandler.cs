using Apps.ModelFront.Api;
using Apps.ModelFront.Constants;
using Apps.ModelFront.Invocables;
using Apps.ModelFront.Models.Response.Languages;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.ModelFront.DataSourceHandlers;

public class LanguageDataHandler : ModelFrontInvocable, IAsyncDataSourceHandler
{
    public LanguageDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new ModelFrontRequest(ApiEndpoints.Languages, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<LanguagesResponse>(request);

        return response.Languages
            .Where(x => context.SearchString is null ||
                        x.Value.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Key, x => x.Value);
    }
}