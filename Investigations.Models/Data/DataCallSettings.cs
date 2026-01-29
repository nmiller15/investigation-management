using System.Data;

namespace Investigations.Models.Data;

public class DataCallSettings
{
    public string ConnectionString { get; set; }
    public string ProcedureName { get; set; }
    public string FunctionName { get; set; }
    public bool IsFunctionCall => !string.IsNullOrEmpty(FunctionName);
    public bool IsProcedureCall => !string.IsNullOrEmpty(ProcedureName);
    public List<DbParam> Parameters = new();

    public void AddParameter(string name, object value)
    {
        DbType dbType;

        if (value == null)
        {
            dbType = DbType.String;
        }
        else
        {
            Type type = value.GetType();
            TypeCode typeCode = Type.GetTypeCode(type);

            dbType = typeCode switch
            {
                TypeCode.String => DbType.String,
                TypeCode.Int16 => DbType.Int16,
                TypeCode.Int32 => DbType.Int32,
                TypeCode.Int64 => DbType.Int64,
                TypeCode.UInt16 => DbType.UInt16,
                TypeCode.UInt32 => DbType.UInt32,
                TypeCode.UInt64 => DbType.UInt64,
                TypeCode.Single => DbType.Single,
                TypeCode.Double => DbType.Double,
                TypeCode.Decimal => DbType.Decimal,
                TypeCode.Boolean => DbType.Boolean,
                TypeCode.DateTime => DbType.DateTime,
                TypeCode.Byte => DbType.Byte,
                TypeCode.SByte => DbType.SByte,
                TypeCode.Char => DbType.String,
                TypeCode.DBNull => DbType.String,
                _ => type == typeof(Guid) ? DbType.Guid :
                     type == typeof(byte[]) ? DbType.Binary :
                     type.IsEnum ? DbType.Int32 :
                     DbType.Object
            };
        }

        Parameters.Add(new DbParam(name, value, dbType));
    }
}
