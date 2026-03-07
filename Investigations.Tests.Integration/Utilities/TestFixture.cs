using System.Reflection;
using Investigations.Configuration;
using Investigations.Features.Auth;
using Npgsql;
using Serilog;
using Testcontainers.PostgreSql;

namespace Investigations.Tests.Integration.Utilities;

public class TestFixture : IAsyncLifetime
{
    public PostgreSqlContainer DbContainer = default!;
    public IServiceProvider Services { get; private set; } = null!;
    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        DbContainer = new PostgreSqlBuilder("postgres:17.5")
            .WithDatabase("core_test")
            .Build();
        await DbContainer.StartAsync();

        ConnectionString = DbContainer.GetConnectionString();

        var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        var services = new ServiceCollection();
        var connStrings = new TestConnectionStrings(ConnectionString, ConnectionString);
        services.AddScoped<IConnectionStrings>(_ => connStrings);

        // register your handlers + dependencies
        services.AddSingleton<IEmailSettings, EmailSettings>();
        services.AddSingleton(new PasswordHasher());

        services.AddRepositories();
        services.AddFeatureHandlers();

        Services = services.BuildServiceProvider();

        await MigrationRunner.Run(connection);
        await MigrationRunner.Seed(connection);
    }

    public async Task DisposeAsync()
    {
        await DbContainer.DisposeAsync();
    }
}
