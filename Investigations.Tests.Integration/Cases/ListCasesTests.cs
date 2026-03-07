using Investigations.Features.Cases;
using Investigations.Tests.Integration.Utilities;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Cases;

public class ListCasesTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ListCases.Handler _handler;
    private readonly ITestOutputHelper _output;

    public ListCasesTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<ListCases.Handler>();
        _output = output;
    }

    [Fact]
    public async Task ListCases_ReturnsCases()
    {
        await CaseSeeder.SeedCases(_fixture, 5);

        var response = await _handler.Handle(new ListCases.Query());

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.NotEmpty(response.Payload.Cases);
        Assert.Equal(5, response.Payload.Cases.Count);

        await CaseSeeder.CleanUp(_fixture);
    }

    [Theory]
    [InlineData("CaseNumber", "ASC")]
    [InlineData("CaseTypeCode", "DESC")]
    [InlineData("", "ASC")]
    public async Task ListCases_SortsCasesBySortColumns(string sortColumn, string sortDirection)
    {
        await CaseSeeder.SeedCases(_fixture, 10);

        var response = await _handler.Handle(new ListCases.Query
        {
            SortColumn = sortColumn,
            SortDirection = sortDirection
        });

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.NotEmpty(response.Payload.Cases);

        if (sortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase))
        {
            Assert.True(response.Payload.Cases.SequenceEqual(response.Payload.Cases.OrderBy(c => c.GetType().GetProperty(sortColumn)?.GetValue(c, null))));
        }
        else if (sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase))
        {
            Assert.True(response.Payload.Cases.SequenceEqual(response.Payload.Cases.OrderByDescending(c => c.GetType().GetProperty(sortColumn)?.GetValue(c, null))));
        }

        await CaseSeeder.CleanUp(_fixture);
    }
}
