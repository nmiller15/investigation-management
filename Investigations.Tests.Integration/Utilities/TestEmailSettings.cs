namespace Investigations.Tests.Integration.Utilities;

public class TestEmailSettings : IEmailSettings
{
    public string Host { get; } = string.Empty;
    public int Port { get; }
    public string Username { get; } = string.Empty;
    public string Password { get; } = string.Empty;
    public bool EnableSsl { get; }
    public string FromAddress { get; } = string.Empty;
    public string FromName { get; } = string.Empty;
    public bool UseDefaultCredentials { get; }
}
