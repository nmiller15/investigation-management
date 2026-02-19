namespace Investigations.Models.Auth;

public record VerifyResult
{
    public bool IsSuccess { get; set; }
    public bool RehashNeeded { get; set; }
}
