using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Cursus.MVC.Models;

namespace Cursus.MVC.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfig _emailConfig;

        public EmailSender(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_emailConfig.SmtpHost, _emailConfig.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailConfig.FromEmail, _emailConfig.FromPassword),
                EnableSsl = _emailConfig.EnableSsl
            };

            await client.SendMailAsync(
                new MailMessage(
                    from: _emailConfig.FromEmail,
                    to: email,
                    subject,
                    body: htmlMessage
                )
                {
                    IsBodyHtml = true
                }
            );
        }

        // Template methods
        public string EmailConfirm(string userName, string confirmationLink)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Email Confirmation</title>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; }}
        .header {{ background-color: #1a82e2; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 20px; }}
        .footer {{ padding: 20px; text-align: center; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to {_emailConfig.CompanyName}</h1>
        </div>
        <div class='content'>
            <p>Hello {userName},</p>
            <p>Please confirm your email address by clicking the link below:</p>
            {confirmationLink}
        </div>
        <div class='footer'>
            <p>&copy; {_emailConfig.CopyrightYear} {_emailConfig.CompanyName}</p>
        </div>
    </div>
</body>
</html>";
        }

        public string EmailNotiConfirmCourse(string fullName, string courseName, string status)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Course {status}</title>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; }}
        .header {{ background-color: #1a82e2; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Course {status}</h1>
        </div>
        <div class='content'>
            <p>Hello {fullName},</p>
            <p>Your course '{courseName}' has been {status.ToLower()}.</p>
            <p>Best regards,<br>{_emailConfig.CompanyName}</p>
        </div>
    </div>
</body>
</html>";
        }

        public string PayOutConfirm(string fullName, double amount)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Payout Confirmation</title>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; }}
        .header {{ background-color: #1a82e2; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Payout Confirmed</h1>
        </div>
        <div class='content'>
            <p>Hello {fullName},</p>
            <p>Your payout of ${amount:F2} has been processed successfully.</p>
            <p>Best regards,<br>{_emailConfig.CompanyName}</p>
        </div>
    </div>
</body>
</html>";
        }

        public string PaymentConfirm(string fullName, double amount)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Payment Confirmation</title>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; }}
        .header {{ background-color: #1a82e2; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Payment Confirmed</h1>
        </div>
        <div class='content'>
            <p>Hello {fullName},</p>
            <p>Your payment of ${amount:F2} has been received successfully.</p>
            <p>Best regards,<br>{_emailConfig.CompanyName}</p>
        </div>
    </div>
</body>
</html>";
        }

        public string EmailChangePassword(string fullName)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Password Changed</title>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
        .container {{ max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; }}
        .header {{ background-color: #1a82e2; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Password Changed</h1>
        </div>
        <div class='content'>
            <p>Hello {fullName},</p>
            <p>Your password has been successfully changed.</p>
            <p>If you did not make this change, please contact us immediately.</p>
            <p>Best regards,<br>{_emailConfig.CompanyName}</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
