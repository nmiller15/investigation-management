namespace Investigations.Configuration;

public interface IConnectionStrings
{
    public string DefaultConnection { get; }
    public string SystemConnection { get; }
}
