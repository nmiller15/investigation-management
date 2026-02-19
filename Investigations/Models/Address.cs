namespace Investigations.Models;

public class Address : BaseAuditModel
{
    public int AddressKey { get; set; } = 0;
    public string LineOne { get; set; } = string.Empty;
    public string LineTwo { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int StateCodeKey { get; set; } = -1;
    public string State { get; set; } = string.Empty;
    public string StateAbbreviation { get; set; } = string.Empty;
    public int CountryCodeKey { get; set; } = -1;
    public string Country { get; set; } = string.Empty;
    public string CountryAbbreviation { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;

    public bool IsNew => AddressKey == 0;
}
