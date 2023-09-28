using Blackbird.Applications.Sdk.Common;

namespace Apps.ModelFront.Models.Request.Predict;

public class RowRequest
{
    [Display("Original text")]
    public string Original { get; set; }
    public string? Translation { get; set; }
}