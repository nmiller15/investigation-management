using Investigations.Features.Contacts;
using Investigations.Tests.Integration.Utilities;
using Npgsql;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Contacts;

public class CreateContactTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly CreateContact.Handler _handler;
    private readonly ITestOutputHelper _output;

    public CreateContactTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<CreateContact.Handler>();
        _output = output;
    }

    [Fact]
    public async Task CreateContact_CreatesContact()
    {
        var cmd = new CreateContact.Command
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "jdoe@sample.email",
            MobilePhone = "555-555-5555",
            WorkPhone = "555-555-5551",
            HomePhone = "555-555-5552",
        };

        var response = await _handler.Handle(cmd);

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Created Contact Key: {0}", response.Payload?.ContactKey);


        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);

        await using (var connection = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await using (var command = new NpgsqlCommand("SELECT first_name, last_name, email, mobile_phone, work_phone, home_phone FROM contacts WHERE contact_key = @ContactKey", connection))
            {
                command.Parameters.AddWithValue("@ContactKey", response.Payload?.ContactKey ?? 0);
                await connection.OpenAsync();
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    Assert.True(await reader.ReadAsync());
                    Assert.Equal(cmd.FirstName, reader.GetString(0));
                    Assert.Equal(cmd.LastName, reader.GetString(1));
                    Assert.Equal(cmd.Email, reader.GetString(2));
                    Assert.Equal(cmd.MobilePhone, reader.GetString(3));
                    Assert.Equal(cmd.WorkPhone, reader.GetString(4));
                    Assert.Equal(cmd.HomePhone, reader.GetString(5));
                }
            }
        }
    }
}

