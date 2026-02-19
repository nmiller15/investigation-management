
namespace Investigations.Configuration;

public class ConnectionStrings : IConnectionStrings
{
    public string DefaultConnection { get; }
    public string SystemConnection { get; }

    public ConnectionStrings(IConfiguration configuration)
    {
        var connectionStrings = configuration.GetSection("ConnectionStrings");

        DefaultConnection = connectionStrings["DefaultConnection"] ?? string.Empty;
        SystemConnection = connectionStrings["SystemConnection"] ?? string.Empty;
    }
}
