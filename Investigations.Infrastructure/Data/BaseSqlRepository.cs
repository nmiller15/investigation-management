using Investigations.Models.Configuration;
using Investigations.Models.Data;
using Investigations.Models.Interfaces;

namespace Investigations.Infrastructure.Data;

public class BaseSqlRepository
{
    private readonly IConnectionStrings _connectionStrings;

    public BaseSqlRepository(IConnectionStrings connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public DataCallSettings GetFunctionCallDcsInstance(string functionName)
    {
        return new DataCallSettings
        {
            ConnectionString = _connectionStrings.DefaultConnection,
            FunctionName = functionName,
        };
    }

    public DataCallSettings GetProcedureCallDcsInstance(string procedureName)
    {
        return new DataCallSettings
        {
            ConnectionString = _connectionStrings.DefaultConnection,
            ProcedureName = procedureName,
        };
    }
}
