using Apps.ModelFront.Api;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.ModelFront.Invocables;

public class ModelFrontInvocable : BaseInvocable
{
    protected AuthenticationCredentialsProvider[] Creds
        => InvocationContext.AuthenticationCredentialsProviders.ToArray();

    protected ModelFrontClient Client { get; }

    public ModelFrontInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new();
    }
}