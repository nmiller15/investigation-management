using System.Net;
using System.Net.Mail;
using Investigations.Models.Configuration;
using Investigations.Models.Email;
using Serilog;

namespace Investigations.Infrastructure.Email;

public static class SmtpSender
{
    public static async Task<bool> SendEmail(IEmailSettings settings, string to, string subject, string body)
    {
        return await SendEmailInternal(settings, new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body,
            IsHtmlBody = false
        });
    }

    public static async Task<bool> SendHtmlEmail(IEmailSettings settings, string to, string subject, string htmlBody)
    {
        return await SendEmailInternal(settings, new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = htmlBody,
            IsHtmlBody = true
        });
    }

    public static async Task<bool> SendEmail(IEmailSettings settings, EmailMessage message)
    {
        return await SendEmailInternal(settings, message);
    }

    private static async Task<bool> SendEmailInternal(IEmailSettings settings, EmailMessage message)
    {
        try
        {
            using var client = new SmtpClient(settings.Host, settings.Port)
            {
                EnableSsl = settings.EnableSsl,
                UseDefaultCredentials = settings.UseDefaultCredentials
            };

            if (!settings.UseDefaultCredentials)
            {
                client.Credentials = new NetworkCredential(settings.Username, settings.Password);
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress(settings.FromAddress, settings.FromName),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsHtmlBody
            };

            mailMessage.To.Add(message.To);

            await client.SendMailAsync(mailMessage);

            Log.Information("Email sent successfully to {ToAddress} with subject {Subject}",
                message.To, message.Subject);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to send email to {ToAddress} with subject {Subject}",
                message.To, message.Subject);
            return false;
        }
    }
}
