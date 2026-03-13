namespace Investigations.Models;

public class PhoneNumber : IContactMethod
{
    public string Value { get; set; }
    public IContactMethod.Types Type { get; set; }
    public bool IsPrimary { get; set; }

    public PhoneNumber(string value, IContactMethod.Types type)
    {
        Value = value;
        Type = type;
    }
}
