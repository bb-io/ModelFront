namespace Apps.ModelFront.Models.Response.Predict;

public class ResponseRow
{
    public float Risk { get; set; }
    
    public float Quality { get; set; }
    
    public string Translation { get; set; }
}