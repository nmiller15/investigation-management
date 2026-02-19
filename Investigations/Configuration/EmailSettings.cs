namespace Investigations.Configuration;

public class EmailSettings : IEmailSettings
{
    public string Host { get; }
    public int Port { get; }
    public string Username { get; }
    public string Password { get; }
    public bool EnableSsl { get; }
    public string FromAddress { get; }
    public string FromName { get; }
    public bool UseDefaultCredentials { get; }

    public EmailSettings(IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");

        Host = emailSettings["Host"] ?? string.Empty;
        Port = int.Parse(emailSettings["Port"] ?? "587");
        Username = emailSettings["Username"] ?? string.Empty;
        Password = emailSettings["Password"] ?? string.Empty;
        EnableSsl = bool.Parse(emailSettings["EnableSsl"] ?? "true");
        FromAddress = emailSettings["FromAddress"] ?? string.Empty;
        FromName = emailSettings["FromName"] ?? string.Empty;
        UseDefaultCredentials = bool.Parse(emailSettings["UseDefaultCredentials"] ?? "false");
    }
}
