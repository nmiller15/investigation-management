using Investigations.Features.Clients;
using Investigations.Tests.Integration.Utilities;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Clients;

public class ListClientsTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ListClients.Handler _handler;
    private readonly ITestOutputHelper _output;

    public ListClientsTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<ListClients.Handler>();
        _output = output;
    }

    [Fact]
    public async Task ListClients_ReturnsClients()
    {
        ClientSeeder.SeedClients(_fixture, 5);

        var response = await _handler.Handle(new ListClients.Query());

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Number of Clients Returned: {0}", response.Payload?.Clients.Count ?? 0);

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.NotEmpty(response.Payload.Clients);

        ClientSeeder.Cleanup(_fixture);
    }

    [Fact]
    public async Task ListClients_InvalidSortColumn_ReturnsValidationError()
    {
        var query = new ListClients.Query
        {
            SortColumn = "InvalidColumn"
        };

        var response = await _handler.Handle(query);

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Validation Error: {0}", response.Message);

        _output.WriteLine("Clients Returned:");

        foreach (var client in response.Payload?.Clients ?? [])
        {
            _output.WriteLine("Client: {0}, PrimaryContactLastName: {1}", client.ClientName, client.PrimaryContactLastName);
        }

        Assert.False(response.WasSuccessful);
        Assert.Equal("Invalid sort column. Valid options are: ClientName, PrimaryContactLastName.", response.Message);
    }

    [Theory]
    [InlineData("ClientName", "ASC")]
    [InlineData("PrimaryContactLastName", "DESC")]
    public async Task ListClients_SortsClientsBySortColumns(string sortColumn, string sortDirection)
    {
        ClientSeeder.SeedClients(_fixture, 10);
        var response = await _handler.Handle(new ListClients.Query
        {
            SortColumn = sortColumn,
            SortDirection = sortDirection
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Number of Clients Returned: {0}", response.Payload?.Clients.Count ?? 0);

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.NotEmpty(response.Payload.Clients);

        _output.WriteLine("Clients Returned:");

        foreach (var client in response.Payload?.Clients ?? [])
        {
            _output.WriteLine("Client: {0}, PrimaryContactLastName: {1}", client.ClientName, client.PrimaryContactLastName);
        }

        if (sortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase))
        {
            Assert.True(response.Payload.Clients.SequenceEqual(response.Payload.Clients.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c, null))));
        }
        else if (sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase))
        {
            Assert.True(response.Payload.Clients.SequenceEqual(response.Payload.Clients.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c, null))));
        }

        ClientSeeder.Cleanup(_fixture);
    }
}
