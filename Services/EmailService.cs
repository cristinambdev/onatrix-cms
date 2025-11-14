using Azure;
using Azure.Communication.Email;
using System.Diagnostics;

namespace Onatrix_Umbraco.Services;

public class EmailService(EmailClient emailClient, IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly EmailClient _emailClient = emailClient;

    public async Task SendEmailAsync(string email, string name = "")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email address cannot be null or empty.", nameof(email));

            var subject = "Message Received";
            var plainTextContent = $@"
            Hi,

            Thank you for contacting us. We will get back to you shortly at <strong>{email}</strong>.
  

            Best Regards
            The Onatrix Team
            
            All rights reserved. Onatrix Inc. {DateTime.UtcNow.Year}
            ";

            var htmlContent = $@"
            <!DOCTYPE html>
            <html>
              <head>
                <meta charset='UTF-8'>
                <title>Message Received</title>
              </head>
              <body style='margin: 0; padding: 0; background-color: #FFFFFF; font-family: Arial, sans-serif;'>
                <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; background-color: #F7F7F7; margin: 20px auto; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                  <tr>
                    <td style='padding: 30px; text-align: center;'>
                    <img 
                        style='max-width:150px; height: auto; display:block; margin:0 auto;'
                        src='https://i.imgur.com/5wRgrtZ.png'
                        alt='Onatrix Logo'  
                    />
                    </td>
                  </tr>
                    <tr>
                    <td style='padding: 10px 0 10px 30px; background-color: #F7F7F7; color: #535656; font-size: 20px; font-weight: bold; border-top-left-radius: 10px; border-top-right-radius: 10px; text-align: left;'>
                      Message Received!
                    </td>
                  </tr>
                  <tr>
                    <td style='padding: 20px 30px 30px 30px; color: #535656;; font-size: 16px; line-height: 1.6;'>
                      <p style='margin-top: 0;'>Hello {name},</p>

                      <p>We’ve received your message. A member of the Onatrix team will reach out to you as soon as possible at <strong>{email}</strong>.</p>

                      <p style='margin-bottom: 0;'>Best Regards,<br>
                      <strong>The Onatrix Team</strong></p>
                    </td>
                  </tr>
                  <tr>
                    <td style='padding: 10px; background-color: #4F5955; text-align: center; color: #F2EDDC; font-size: 12px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;'>
                      &copy; {DateTime.UtcNow.Year} Onatrix. All rights reserved.
                    </td>
                  </tr>
                </table>
              </body>
            </html>
            ";

            var emailMessage = new Azure.Communication.Email.EmailMessage(
                senderAddress: _configuration["ACS:SenderAddress"],
                recipients: new EmailRecipients([new(email)]),
                content: new EmailContent(subject)
                {
                    PlainText = plainTextContent,
                    Html = htmlContent
                });

            await _emailClient.SendAsync(WaitUntil.Started, emailMessage);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error sending the email {ex.Message}");
        }

    }
}