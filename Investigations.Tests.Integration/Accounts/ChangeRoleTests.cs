using Bogus;
using Investigations.Features.Account;
using Investigations.Models;
using Investigations.Tests.Integration.Utilities;
using Npgsql;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Accounts;

public class ChangeRoleTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ChangeRole.Handler _handler;
    private readonly ITestOutputHelper _output;

    public ChangeRoleTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<ChangeRole.Handler>();
        _output = output;
    }

    [Fact]
    public async Task ChangeRole_SuccessfullyChangesRole()
    {
        //Arrange
        var user = await UserSeeder.SeedSingleUser(_fixture);
        var expectedRole = User.Roles.AccountOwner;

        //Act
        var response = await _handler.Handle(new ChangeRole.Command
        {
            UserKey = user.UserKey,
            Role = expectedRole,
            CurrentUserKey = 100,
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Response.Message: {0}", response.Message);

        //Assert
        Assert.True(response.WasSuccessful);

        await using (var connection = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await connection.OpenAsync();
            await using (var cmd = new NpgsqlCommand("SELECT role_code_key FROM users WHERE user_key = @UserKey;", connection))
            {
                cmd.Parameters.AddWithValue("UserKey", user.UserKey);
                var newRoleCodeKey = (int)await cmd.ExecuteScalarAsync();
                Assert.Equal((int)expectedRole, newRoleCodeKey);
            }
        }
    }
}
