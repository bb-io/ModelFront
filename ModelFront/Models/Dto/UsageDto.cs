using Blackbird.Applications.Sdk.Common;

namespace Apps.ModelFront.Models.Dto;

public class UsageDto
{
    [Display("Prompt tokens")] 
    public int PromptTokens { get; set; }

    [Display("Candidates tokens")] 
    public int CandidatesTokens { get; set; }

    [Display("Total tokens")] 
    public int TotalTokens { get; set; }

    public static UsageDto operator +(UsageDto u1, UsageDto u2)
    {
        return new UsageDto
        {
            PromptTokens = u1.PromptTokens + u2.PromptTokens,
            CandidatesTokens = u1.CandidatesTokens + u2.CandidatesTokens,
            TotalTokens = u1.TotalTokens + u2.TotalTokens,
        };
    }

    public UsageDto() { }
}
