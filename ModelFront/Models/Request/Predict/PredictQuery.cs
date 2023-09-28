using Apps.ModelFront.DataSourceHandlers;
using Apps.ModelFront.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.ModelFront.Models.Request.Predict;

public class PredictQuery
{
    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    [JsonProperty("sl")]
    public string SourceLanguage { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    [JsonProperty("tl")]
    public string TargetLanguage { get; set; }

    [DataSource(typeof(EngineDataHandler))]
    [JsonProperty("engine")]
    public string? Engine { get; set; }

    [JsonProperty("model")] public string? Model { get; set; }

    [Display("Custom engine ID")]
    [JsonProperty("custom_engine_id")]
    public string? CustomEngineId { get; set; }

    [Display("Engine key")]
    [JsonProperty("engine_key")]
    public string? EngineKey { get; set; }

    [Display("Metadata ID")]
    [JsonProperty("metadata_id")]
    public string? MetadataId { get; set; }
}