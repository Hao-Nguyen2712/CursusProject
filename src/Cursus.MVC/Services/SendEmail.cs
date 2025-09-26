using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Cursus.MVC.Models;

namespace Cursus.MVC.Services
{
    public class SendEmail : ISendEmail
    {
        private readonly EmailConfig _emailConfig;

        public SendEmail(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public string GetEmailName(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                return email.Substring(0, atIndex);
            }
            else
            {
                throw new ArgumentException("Invalid email format", nameof(email));
            }
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_emailConfig.SmtpHost, _emailConfig.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailConfig.FromEmail, _emailConfig.FromPassword),
                EnableSsl = _emailConfig.EnableSsl
            };
            
            string userName = GetEmailName(email);
            return client.SendMailAsync(
                new MailMessage(
                    from: _emailConfig.FromEmail,
                    to: email,
                    subject,
                    body: htmlMess(userName, message)
                    )
                {
                    IsBodyHtml = true
                }
            );
        }

        public string htmlMess(string userName, string message)
        {
            string form = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta http-equiv='x-ua-compatible' content='ie=edge'>
    <title>Email Notification</title>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <style type='text/css'>
        body {{ background-color: #e9ecef; font-family: Arial, sans-serif; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; }}
        .header {{ background-color: #1a82e2; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 30px; }}
        .footer {{ padding: 20px; text-align: center; color: #666; background-color: #f8f9fa; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; padding: 12px 24px; background-color: #1a82e2; color: white; text-decoration: none; border-radius: 4px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Notification from {_emailConfig.CompanyName}</h1>
        </div>
        <div class='content'>
            <p>Dear {userName},</p>
            <p>{message}</p>
            <p>Best regards,<br>{_emailConfig.CompanyName}</p>
        </div>
        <div class='footer'>
            <p>You received this email from {_emailConfig.CompanyName}.</p>
            <p>{_emailConfig.CompanyName} - {_emailConfig.CompanyAddress}</p>
            <p>&copy; {_emailConfig.CopyrightYear} {_emailConfig.CompanyName}. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
            return form;
        }
    }
}
