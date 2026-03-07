using Bogus;
using Investigations.Features.Account;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;
using Investigations.Tests.Integration.Utilities;
using Npgsql;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Accounts;

public class EditAccountTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly EditAccount.Handler _handler;
    private readonly ITestOutputHelper _output;

    public EditAccountTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<EditAccount.Handler>();
        _output = output;
    }

    [Fact]
    public async Task EditAccount_EditsAccountSuccessfully()
    {
        //Arrange
        var user = await Seed();
        var editedUser = new User
        {
            UserKey = user.UserKey,
            FirstName = user.FirstName + "Edited",
            LastName = user.LastName + "Edited",
            Email = user.Email + "Edited",
            Birthdate = user.Birthdate.GetValueOrDefault(DateTime.Parse("01/01/1990")).AddDays(1)
        };

        //Act
        var response = await _handler.Handle(new EditAccount.Command
        {
            UserKey = editedUser.UserKey,
            FirstName = editedUser.FirstName,
            LastName = editedUser.LastName,
            Email = editedUser.Email,
            Birthdate = editedUser.Birthdate.GetValueOrDefault(),
            CurrentUserKey = user.UserKey
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Response.Message: {0}", response.Message);

        Assert.True(response.WasSuccessful);

        await using (var conn = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await conn.OpenAsync();
            await using (var cmd = new NpgsqlCommand("SELECT first_name, last_name, email, birthdate FROM users WHERE user_key = @UserKey;", conn))
            {
                cmd.Parameters.AddWithValue("UserKey", editedUser.UserKey);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Assert.True(await reader.ReadAsync());
                    Assert.Equal(editedUser.FirstName, reader.ParseString("first_name"));
                    Assert.Equal(editedUser.LastName, reader.ParseString("last_name"));
                    Assert.Equal(editedUser.Email, reader.ParseString("email"));
                    Assert.Equal(editedUser.Birthdate, reader.ParseDateTime("birthdate"));
                }
            }
        }
    }

    private async Task<User> Seed()
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

        await using (var conn = new NpgsqlConnection(_fixture.ConnectionString))
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
                user.UserKey = (int)(await cmd.ExecuteScalarAsync());
            }
        }

        return user;
    }
}
