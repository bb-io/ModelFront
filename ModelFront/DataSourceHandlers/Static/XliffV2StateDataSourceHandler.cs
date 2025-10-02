using Blackbird.Filters.Enums;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.ModelFront.DataSourceHandlers.Static;

public class XliffV2StateDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new(SegmentState.Initial.Serialize(), "Initial or empty"),
        new(SegmentState.Translated.Serialize(), "Translated"),
        new(SegmentState.Reviewed.Serialize(), "Reviewed"),
        new(SegmentState.Final.Serialize(), "Final"),
    ];
}