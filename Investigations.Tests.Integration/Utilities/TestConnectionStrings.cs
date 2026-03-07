using Investigations.Configuration;

namespace Investigations.Tests.Integration.Utilities;

public class TestConnectionStrings : IConnectionStrings
{
    public string DefaultConnection { get; }
    public string SystemConnection { get; }

    public TestConnectionStrings(string defaultConnection, string systemConnection)
    {
        DefaultConnection = defaultConnection;
        SystemConnection = systemConnection;
    }
}
