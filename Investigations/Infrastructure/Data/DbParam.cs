using System.Data;

namespace Investigations.Infrastructure.Data;

public sealed record DbParam(
        string Name,
        object? Value,
        DbType? Type = null,
        ParameterDirection Direction = ParameterDirection.Input
);
