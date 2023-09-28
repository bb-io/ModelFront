namespace Apps.ModelFront.Models.Request.Predict;

public class PredictRequest
{
    public IEnumerable<RowRequest> Rows { get; set; }
}