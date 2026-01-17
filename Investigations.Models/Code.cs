namespace Investigations.Models;

public class Code : BaseModelWithAudit
{
    public int CodeKey { get; set; } = 0;
    public string CodeValue { get; set; } = string.Empty;
    public string CodeType { get; set; } = string.Empty;
    public string CodeDescription { get; set; } = string.Empty;
    public string CodeShortDescription { get; set; } = string.Empty;
}
