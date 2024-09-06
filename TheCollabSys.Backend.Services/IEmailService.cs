namespace TheCollabSys.Backend.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendEmailToSupportAsync(string userEmail, string subject, string body);
}