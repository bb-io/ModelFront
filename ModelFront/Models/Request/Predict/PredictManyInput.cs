namespace Apps.ModelFront.Models.Request.Predict;

public class PredictManyInput
{
    public IEnumerable<RowRequest> Segments { get; set; }
}