using Investigations.Features.Contacts;
using Investigations.Tests.Integration.Utilities;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Contacts;

public class ListContactsTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ListContacts.Handler _handler;
    private readonly ITestOutputHelper _output;

    public ListContactsTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<ListContacts.Handler>();
        _output = output;
    }

    [Fact]
    public async Task ListContacts_ReturnsContacts()
    {
        ContactSeeder.SeedContacts(_fixture, 5);

        var response = await _handler.Handle(new ListContacts.Query());

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Number of Contacts Returned: {0}", response.Payload?.Contacts.Count ?? 0);

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.NotEmpty(response.Payload.Contacts);

        ContactSeeder.Cleanup(_fixture);
    }

    [Fact]
    public async Task ListContacts_InvalidSortColumn_ReturnsValidationError()
    {
        var query = new ListContacts.Query
        {
            SortColumn = "InvalidColumn"
        };

        var response = await _handler.Handle(query);

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Validation Error: {0}", response.Message);

        Assert.False(response.WasSuccessful);
        Assert.Equal("Invalid sort column. Valid options are: FirstName, LastName, Email.", response.Message);
    }

    [Theory]
    [InlineData("FirstName", "ASC")]
    [InlineData("LastName", "DESC")]
    [InlineData("Email", "ASC")]
    public async Task ListContacts_SortsContactsBySortColumns(string sortColumn, string sortDirection)
    {
        ContactSeeder.SeedContacts(_fixture, 10);
        var response = await _handler.Handle(new ListContacts.Query
        {
            SortColumn = sortColumn,
            SortDirection = sortDirection
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Number of Contacts Returned: {0}", response.Payload?.Contacts.Count ?? 0);

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.NotEmpty(response.Payload.Contacts);

        if (sortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase))
        {
            Assert.True(response.Payload.Contacts.SequenceEqual(response.Payload.Contacts.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c, null))));
        }
        else if (sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase))
        {
            Assert.True(response.Payload.Contacts.SequenceEqual(response.Payload.Contacts.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c, null))));
        }

        ContactSeeder.Cleanup(_fixture);
    }
}
