namespace Investigations.Models;

public class Client : BaseAuditModel
{
    public Guid ClientId { get; set; } = Guid.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public Guid AddressId { get; set; } = Guid.Empty;
    public string LineOne { get; set; } = string.Empty;
    public string LineTwo { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
}
