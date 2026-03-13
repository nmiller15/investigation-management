namespace Investigations;

public interface IContactMethod
{
    public enum Types
    {
        Email = 202,
        Mobile = 203,
        Work = 204,
        Home = 205
    }

    public Types Type { get; }
    public bool IsPrimary { get; set; }
}
