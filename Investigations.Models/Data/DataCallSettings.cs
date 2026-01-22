namespace Investigations.Models.Data;

public class DataCallSettings
{
    public string ConnectionString { get; set; }
    public string ProcedureName { get; set; }
    public Dictionary<string, object> Parameters = new Dictionary<string, object>();

    public void AddParameter(string name, object value)
    {
        Parameters.Add(name, value);
    }
}
