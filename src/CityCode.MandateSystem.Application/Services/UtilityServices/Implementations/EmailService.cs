using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CityCode.MandateSystem.Application.Services.UtilityServices.Implementations;
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<SystemSettings> systemSettings)
    {
        _emailSettings = systemSettings.Value.EmailSettings!;
    }

    public async Task<bool> SendEmail(string toEmail, MailContent mailContent)
    {
        try
        {
            var client = new SendGridClient(_emailSettings.SendGridApiKey);
            var from = new EmailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
            var to = new EmailAddress(toEmail);

            var subject = mailContent.Subject;
            var htmlBody = $@"
                <h1>{mailContent.Header}</h1>
                <br/>
                <p>{mailContent.Body}</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "", htmlContent: htmlBody);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Notification failed with error: {ex.Message}");
            return false;
        }
    }
}
