using System.Text;
using Npgsql;
using Serilog;

namespace Investigations.Tests.Integration.Utilities;

public static class MigrationRunner
{
    public async static Task Run(NpgsqlConnection connection)
    {
        using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS schema_migrations"
                            + "(version VARCHAR(255) PRIMARY KEY, "
                            + "applied_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP);",
                            connection))
        {
            cmd.ExecuteNonQuery();
        }

        using (var cmd = new NpgsqlCommand("CREATE ROLE app_user;", connection))
        {
            cmd.ExecuteNonQuery();
        }

        var applied = new HashSet<string>();

        using (var cmd = new NpgsqlCommand("SELECT version FROM schema_migrations;", connection))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
                applied.Add(reader.GetString(0));
        }


        var migrationFiles = Directory
            .GetFiles(AppContext.BaseDirectory, "*.sql")
            .OrderBy(f => f);

        var pendingMigrations = migrationFiles
            .Where(version => !applied.Contains(Path.GetFileNameWithoutExtension(version)) ||
                              Path.GetFileNameWithoutExtension(version).Equals("001_codes"))
            .ToList();

        Log.Debug($"Applying {pendingMigrations.Count} pending migrations...");

        foreach (var migration in pendingMigrations)
        {
            var version = Path.GetFileNameWithoutExtension(migration);
            var sql = File.ReadAllText(migration);

            using var transaction = connection.BeginTransaction();
            try
            {
                var sb = new StringBuilder();
                sb.Append("Applying migration " + version + "... ");
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
                sb.AppendLine("done!");
                Log.Debug(sb.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("Failed to apply migration {Version}: {ErrorMessage}", version, ex.Message);
                transaction.Rollback();
            }
        }
    }

    public async static Task Seed(NpgsqlConnection connection)
    {
        var codesSeed = Path.Combine(AppContext.BaseDirectory + "001_codes.sql");

        using (var cmd = new NpgsqlCommand(File.ReadAllText(codesSeed), connection))
        {
            cmd.ExecuteNonQuery();
        }

        using (var cmd = new NpgsqlCommand("GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO app_user;"
                                            + "GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO app_user;", connection))
        {
            cmd.ExecuteNonQuery();
        }
    }
}
