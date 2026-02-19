namespace Investigations;

public interface IEmailSettings
{
    public string Host { get; }
    public int Port { get; }
    public string Username { get; }
    public string Password { get; }
    public bool EnableSsl { get; }
    public string FromAddress { get; }
    public string FromName { get; }
    public bool UseDefaultCredentials { get; }
}
