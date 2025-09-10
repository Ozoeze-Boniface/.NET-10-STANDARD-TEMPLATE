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
                        <!DOCTYPE html>
                        <html>
                        <head>
                        <meta charset='UTF-8'>
                        <title>{mailContent.Subject}</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px;'>
                        <table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px; margin:auto; background:#ffffff; border-radius:6px; border:1px solid #e0e0e0;'>
                            <tr>
                            <td style='padding: 20px;'>
                                
                                <h2 style='color:#333333; margin-top:0;'>{mailContent.Header}</h2>
                                
                                <p style='color:#555555; font-size:15px; line-height:1.6; margin-bottom:20px;'>
                                {mailContent.Body}
                                </p>
                                
                                <hr style='border:none; border-top:1px solid #e0e0e0; margin:20px 0;'/>
                                
                                <p style='font-size:14px; color:#777777;'>
                                Thank you,<br/>
                                <strong>{_emailSettings.FromName}</strong>
                                </p>
                            </td>
                            </tr>
                        </table>
                        </body>
                        </html>";

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
