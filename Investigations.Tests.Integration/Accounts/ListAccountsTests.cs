using Bogus;
using Investigations.Features.Account;
using Investigations.Models;
using Investigations.Tests.Integration.Utilities;
using Npgsql;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Accounts;

public class ListAccountsTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ListAccounts.Handler _handler;
    private readonly ITestOutputHelper _output;

    public ListAccountsTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<ListAccounts.Handler>();
        _output = output;
    }

    [Fact]
    public async Task ListAccounts_GetsListOfAccounts()
    {
        //Arrange
        await Seed();

        //Act
        var response = await _handler.Handle(new ListAccounts.Query
        {
            SortColumn = "LastName",
            SortDirection = "ASC"
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Response.Message: {0}", response.Message);
        _output.WriteLine("Response.Payload.Count: {0}", response.Payload?.Accounts.Count);

        //Assert
        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.Equal(3, response.Payload.Accounts.Count);

        await CleanUp();
    }

    [Theory]
    [InlineData("LastName", "ASC")]
    [InlineData("LastName", "DESC")]
    [InlineData("Email", "ASC")]
    [InlineData("Email", "DESC")]
    private async Task ListAccounts_SortsByInputParameters(string sortColumn, string sortDirection)
    {
        //Arrange
        await Seed(10);

        //Act
        var response = await _handler.Handle(new ListAccounts.Query
        {
            SortColumn = sortColumn,
            SortDirection = sortDirection
        });

        //Assert
        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);

        for (var i = 0; i < response.Payload.Accounts.Count; i++)
        {
            var account = response.Payload.Accounts[i];
            _output.WriteLine("UserKey: {0}, LastName: {1}, FirstName: {2}, Email: {3}, Role: {4}",
                account.UserKey, account.LastName, account.FirstName, account.Email, account.Role);
            if (i > 0)
            {
                var previousAccount = response.Payload.Accounts[i - 1];
                var comparison = sortColumn.ToLower() switch
                {
                    "lastname" => string.Compare(previousAccount.LastName, account.LastName, StringComparison.OrdinalIgnoreCase),
                    "email" => string.Compare(previousAccount.Email, account.Email, StringComparison.OrdinalIgnoreCase),
                };
                var comparisonMatchesDirection = sortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase)
                    ? comparison < 0
                    : comparison > 0;

                Assert.True(comparison == 0 || comparisonMatchesDirection,
                    $"Accounts are not sorted correctly by {sortColumn} in {sortDirection} order.");
            }
        }
        await CleanUp();
    }

    private async Task Seed(int numberOfUsers = 3)
    {
        var testUsers = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Role, f => User.Roles.Investigator)
            .RuleFor(u => u.InsertedByUserKey, f => 100)
            .RuleFor(u => u.InsertedDateTime, f => DateTime.UtcNow)
            .Generate(numberOfUsers);

        await using (var conn = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await using (var cmd = new NpgsqlCommand("INSERT INTO users (first_name, last_name, email, password_hash, role_code_key, inserted_by_user_key, inserted_datetime)" +
                        "VALUES (@firstName, @lastName, @email, '', @roleCodeKey, @insertedByUserKey, @insertedDateTime);", conn))
            {
                await conn.OpenAsync();
                foreach (var user in testUsers)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("firstName", user.FirstName);
                    cmd.Parameters.AddWithValue("lastName", user.LastName);
                    cmd.Parameters.AddWithValue("email", user.Email);
                    cmd.Parameters.AddWithValue("roleCodeKey", (int)user.Role);
                    cmd.Parameters.AddWithValue("insertedByUserKey", user.InsertedByUserKey);
                    cmd.Parameters.AddWithValue("insertedDateTime", user.InsertedDateTime);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }

    private async Task CleanUp()
    {
        await using (var conn = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await conn.OpenAsync();
            await using (var cmd = new NpgsqlCommand("TRUNCATE TABLE users;", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
