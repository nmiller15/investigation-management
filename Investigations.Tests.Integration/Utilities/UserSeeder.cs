using Bogus;
using Investigations.Models;
using Npgsql;

namespace Investigations.Tests.Integration.Utilities;

public class UserSeeder
{
    public async static Task<List<User>> SeedUsers(TestFixture fixture, int numberOfUsers)
    {
        var users = new List<User>();
        for (int i = 0; i < numberOfUsers; i++)
        {
            var user = await SeedSingleUser(fixture);
            users.Add(user);
        }
        return users;
    }

    public async static Task<User> SeedSingleUser(TestFixture fixture)
    {
        var user = new Faker<User>()
            .RuleFor(u => u.UserKey, f => 0)
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Role, f => User.Roles.Investigator)
            .RuleFor(u => u.InsertedByUserKey, f => 100)
            .RuleFor(u => u.InsertedDateTime, f => DateTime.UtcNow)
            .Generate();

        await using (var conn = new NpgsqlConnection(fixture.ConnectionString))
        {
            await using (var cmd = new NpgsqlCommand("INSERT INTO users (first_name, last_name, email, password_hash, role_code_key, inserted_by_user_key, inserted_datetime)" +
                        "VALUES (@firstName, @lastName, @email, '', @roleCodeKey, @insertedByUserKey, @insertedDateTime)"
                        + "RETURNING user_key;", conn))
            {
                await conn.OpenAsync();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("firstName", user.FirstName);
                cmd.Parameters.AddWithValue("lastName", user.LastName);
                cmd.Parameters.AddWithValue("email", user.Email);
                cmd.Parameters.AddWithValue("roleCodeKey", (int)user.Role);
                cmd.Parameters.AddWithValue("insertedByUserKey", user.InsertedByUserKey);
                cmd.Parameters.AddWithValue("insertedDateTime", user.InsertedDateTime);
                user.UserKey = (int)(await cmd.ExecuteScalarAsync() ?? 0);
            }
        }

        return user;
    }

    public async static Task CleanUp(TestFixture fixture)
    {
        await using (var connection = new NpgsqlConnection(fixture.ConnectionString))
        {
            await using (var cmd = new NpgsqlCommand("TRUNCATE TABLE users;", connection))
            {
                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
