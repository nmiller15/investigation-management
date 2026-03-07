using Bogus;
using Investigations.Features.Account;
using Investigations.Features.Auth;
using Investigations.Models;
using Investigations.Tests.Integration.Utilities;
using Npgsql;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Accounts;

public class ChangePasswordTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ChangePassword.Handler _handler;
    private readonly ITestOutputHelper _output;

    private const string SeedPassword = "Password123!";
    private const string NewPassword = "Password321!";

    public ChangePasswordTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<ChangePassword.Handler>();
        _output = output;
    }

    [Theory]
    [InlineData(SeedPassword, true)]
    [InlineData("DifferentPassword", false)]
    public async Task ChangePassword_ChangesPasswordSuccessfully(string currentPassword, bool expectedOutcome)
    {
        //Arrange
        var user = await Seed();

        // Act
        var response = await _handler.Handle(new ChangePassword.Command
        {
            UserKey = user.UserKey,
            CurrentPassword = currentPassword,
            NewPassword = NewPassword,
            CurrentUserKey = user.UserKey
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Response.Message: {0}", response.Message);

        // Assert
        Assert.Equal(expectedOutcome, response.WasSuccessful);

        await using (var conn = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await conn.OpenAsync();
            await using (var cmd = new NpgsqlCommand("SELECT password_hash FROM users WHERE user_key = @UserKey;", conn))
            {
                cmd.Parameters.AddWithValue("UserKey", user.UserKey);
                var newHash = (string)await cmd.ExecuteScalarAsync();
                var verifyResult = new PasswordHasher().Verify(newHash, NewPassword);
                Assert.Equal(expectedOutcome, verifyResult.IsSuccess);
            }
        }

        await CleanUp();
    }

    private async Task<User> Seed()
    {
        var user = new Faker<User>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .Generate();

        var passwordHash = new PasswordHasher().Hash(SeedPassword);

        using (var connection = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await using (var cmd = new NpgsqlCommand("INSERT INTO users (first_name, last_name, email, password_hash, role_code_key, inserted_by_user_key, inserted_datetime)" +
                        "VALUES (@firstName, @lastName, @email, @password_hash, @roleCodeKey, @insertedByUserKey, @insertedDateTime);", connection))
            {
                await connection.OpenAsync();
                cmd.Parameters.AddWithValue("firstName", user.FirstName);
                cmd.Parameters.AddWithValue("lastName", user.LastName);
                cmd.Parameters.AddWithValue("email", user.Email);
                cmd.Parameters.AddWithValue("password_hash", passwordHash);
                cmd.Parameters.AddWithValue("roleCodeKey", (int)User.Roles.Investigator);
                cmd.Parameters.AddWithValue("insertedByUserKey", 100);
                cmd.Parameters.AddWithValue("insertedDateTime", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
            }
        }

        var userKey = 0;
        using (var connection = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await using (var cmd = new NpgsqlCommand("SELECT user_key FROM users WHERE email = @Email;", connection))
            {
                await connection.OpenAsync();
                cmd.Parameters.AddWithValue("Email", user.Email);
                user.UserKey = (int)cmd.ExecuteScalar();
            }
        }

        return user;
    }

    private async Task CleanUp()
    {
        using (var connection = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await using (var cmd = new NpgsqlCommand("TRUNCATE TABLE users;", connection))
            {
                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
