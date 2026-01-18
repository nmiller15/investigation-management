using System.Text.Json;
using Npgsql;

namespace Investigations.Db;

public static class Program
{
    public static void Main(string[] args)
    {
        var connectionString = GetConnectionString(args);

        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        var applied = new HashSet<string>();

        using (var cmd = new NpgsqlCommand("SELECT version FROM schema_migrations;", connection))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
                applied.Add(reader.GetString(0));
        }

        var migrationFiles = Directory
            .GetFiles("Migrations", "*.sql")
            .OrderBy(f => f);

        var pendingMigrations = migrationFiles
            .Where(version => !applied.Contains(Path.GetFileNameWithoutExtension(version)))
            .ToList();

        Console.WriteLine($"Applying {pendingMigrations.Count} pending migrations...");

        foreach (var migration in pendingMigrations)
        {
            var version = Path.GetFileNameWithoutExtension(migration);
            var sql = File.ReadAllText(migration);

            using var transaction = connection.BeginTransaction();
            try
            {
                Console.Write("Applying migration " + version + "... ");
                using (var cmd = new NpgsqlCommand(sql, connection, transaction))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var recordCmd = new NpgsqlCommand("INSERT INTO schema_migrations (version) VALUES (@version);", connection, transaction))
                {
                    recordCmd.Parameters.AddWithValue("version", version);
                    recordCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                Console.WriteLine("done!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(ex.Message);
                transaction.Rollback();
                System.Environment.Exit(1);
            }
        }
    }

    public static string GetConnectionString(string[] args)
    {
        var environment = "development";
        if (args.Length > 1 && args[0] == "--prod")
        {
            environment = "production";
        }

        var configFilePath = $"{environment}.connectionStrings.json";
        if (!File.Exists(configFilePath))
        {
            Console.WriteLine("No connection string configuration file found.");
            System.Environment.Exit(1);
        }

        string json = File.ReadAllText(configFilePath);
        var settings = JsonSerializer.Deserialize<AppSettings>(json);
        var connectionString = settings.ConnectionStrings.SystemConnection;

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("No connection string found in configuration file.");
            System.Environment.Exit(1);
        }

        return connectionString;
    }
}

public class AppSettings()
{
    public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
}

public class ConnectionStrings()
{
    public string DefaultConnection { get; set; } = string.Empty;
    public string SystemConnection { get; set; } = string.Empty;
}
