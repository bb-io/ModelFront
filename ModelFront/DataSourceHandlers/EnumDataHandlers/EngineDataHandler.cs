using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.ModelFront.DataSourceHandlers.EnumDataHandlers;

public class EngineDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "google", "Google" },
        { "microsoft", "Microsoft" },
        { "deepl", "DeepL" },
        { "modernmt", "ModernMT" },
        { "*", "Let ModelFront choose" },
    };
}