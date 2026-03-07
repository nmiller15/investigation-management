using Investigations.Models;

namespace Investigations.Tests.Unit;

public class CaseTests
{
    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void IsNew_BasedOnKeyValue(int caseKey, bool expected)
    {
        var caseInstance = new Case { CaseKey = caseKey };

        Assert.Equal(expected, caseInstance.IsNew);
    }
}
