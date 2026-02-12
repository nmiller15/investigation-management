namespace Investigations.Models.Configuration;

public interface IEmailSettings
{
    string Host { get; }
    int Port { get; }
    string Username { get; }
    string Password { get; }
    bool EnableSsl { get; }
    string FromAddress { get; }
    string FromName { get; }
    bool UseDefaultCredentials { get; }
}
