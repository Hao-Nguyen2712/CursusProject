using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using static System.Net.Mime.MediaTypeNames;

namespace Cursus.MVC.Service
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmail = "tranthaitansang23122003@gmail.com";
            var fromPassword = "qarmfksczdzispzo";
            var message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.Subject = subject;
            message.To.Add(email);
            message.Body = $"<html><body> {htmlMessage} </body></html>";
            message.IsBodyHtml = true;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };
            smtpClient.Send(message);
        }
        public static string EmailConfirm(string username, string content)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                        .link-confirm {{
                            display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Confirm Your Email Address</h1>
                        <div class='content'>
                            <p>Hello {username},</p>
                            <p>Thank you for being a valued member of our community.</p>    
                            {content}
                            <p>If you have any questions or require further assistance, please do not hesitate to contact our support team.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 tranthaitansang23122003@gmail.com. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";
            return Response;
        }

        public static string EmailChangePassword(string username)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                        .link-confirm {{
                            display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Change Passowrd</h1>
                        <div class='content'>
                            <p>Hello {username},</p>
                            <p>Thank you for being a valued member of our community.</p>
                            YOU HAVE CHANGE YOUR PASSWORD.
                            <p>If you have any questions or require further assistance, please do not hesitate to contact our support team.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 datdtce171751@fpt.edu.vn. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";
            return Response;
        }

        public static string PaymentConfirm(string username, int money)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                        .link-confirm {{
                            display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Confirm payment to account</h1>
                        <div class='content'>
                            <p>Hello {username},</p>
                            <p>Confirm successful payment to account </p>    
                             <h4> amount of money: {money} </h4>
                            <p>for your account.</p>
                            <p>Thank you for using our service.</p>
                            <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";
            return Response;
        }

        public static string PayOutConfirm(string username, int money)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                        .link-confirm {{
                            display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Confirm payment to account</h1>
                        <div class='content'>
                            <p>Hello {username},</p>
                            <p>You have successfully withdrawn</p>    
                             <h4> amount of money: {money} </h4>
                            <p>in  your account.</p>
                            <p>Thank you for using our service.</p>
                            <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";
            return Response;
        }

        public static string Enroll(string username, string courseName)
        {
            string Response = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Account Updated</title>
            <style>
                /* Reset CSS */
                body, html {{
                    margin: 0;
                    padding: 0;
                    font-family: Arial, sans-serif;
                }}
                /* Container */
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    padding: 20px;
                    background-color: #f9f9f9;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                /* Header */
                .container h1 {{
                    text-align: center;
                    margin: 0; 
                    font-size: 32px; 
                    font-weight: 700; 
                    letter-spacing: -1px; 
                    line-height: 48px;
                }}
                /* Content */
                .content {{
                    background-color: #fff;
                    padding: 20px;
                    border-radius: 5px;
                    margin-top: 20px;
                }}
                /* Footer */
                .footer {{
                    text-align: center;
                    margin-top: 20px;
                }}
                .footer p {{
                    color: #888;
                }}
                .link-confirm {{
                    display: inline-block;
                      background-color: #007bff;
                      color: white;
                      padding: 10px 20px;
                      text-align: center;
                      border-radius: 5px;
                      text-decoration: none;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Confirm registered course</h1>
                <div class='content'>
                    <h4>Notification</h4>
                    <p>Hello {username},</p>
                    <p>You have enrolled the course:</p>    
                     <h4> {courseName} </h4>
                    <p>Thank you for using our service.</p>
                    <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                    <p>Best regards,</p>
                    <p>Cursus</p>                            
                </div>
                <div class='footer'>
                    <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                </div>
            </div>
        </body>
        </html>";
            return Response;
        }
        public static string UnEnroll(string username, string courseName)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                        .link-confirm {{
                            display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Confirm payment to account</h1>
                        <div class='content'>
                            <h4>Notification</h4>
                            <p>Hello {username},</p>
                            <p>You have unenroll the course:</p>    
                             <h4> {courseName} </h4>
                            <p>Thank you for using our service.</p>
                            <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";
            return Response;
        }

        public static string BuyNow(string username)
        {
            string Response = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Account Updated</title>
                <style>
                    /* Reset CSS */
                    body, html {{
                        margin: 0;
                        padding: 0;
                        font-family: Arial, sans-serif;
                    }}
                    /* Container */
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        background-color: #f9f9f9;
                        border-radius: 10px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    /* Header */
                    .container h1 {{
                        text-align: center;
                        margin: 0; 
                        font-size: 32px; 
                        font-weight: 700; 
                        letter-spacing: -1px; 
                        line-height: 48px;
                    }}
                    /* Content */
                    .content {{
                        background-color: #fff;
                        padding: 20px;
                        border-radius: 5px;
                        margin-top: 20px;
                    }}
                    /* Footer */
                    .footer {{
                        text-align: center;
                        margin-top: 20px;
                    }}
                    .footer p {{
                        color: #888;
                    }}
                    .link-confirm {{
                        display: inline-block;
                        background-color: #007bff;
                        color: white;
                        padding: 10px 20px;
                        text-align: center;
                        border-radius: 5px;
                        text-decoration: none;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Successfully purchased the course</h1>
                    <div class='content'>
                        <h4>Notification</h4>
                        <p>Hello {username},</p>
                        <p>You have successfully purchased the course.</p>    
                        <p>Thank you for using our service.</p>
                        <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                        <p>Best regards,</p>
                        <p>Cursus</p>                            
                    </div>
                    <div class='footer'>
                        <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                    </div>
                </div>
            </body>
            </html>";
            return Response;
        }

        public static string AdminNotification(string username)
        {
            string Response = @"
<!DOCTYPE html>
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
</head>

<body>
    <div style='margin:0;padding:0' bgcolor='#FFFFFF'>
        <table width='100%' height='100%' style='min-width:348px' border='0' cellspacing='0' cellpadding='0' lang='en'>
            <tbody>
                <tr height='32' style='height:32px'>
                    <td></td>
                </tr>
                <tr align='center'>
                    <td>
                        <div>
                            <div></div>
                        </div>
                        <table border='0' cellspacing='0' cellpadding='0'
                            style='padding-bottom:20px;max-width:516px;min-width:220px'>
                            <tbody>
                                <tr>
                                    <td width='8' style='width:8px'></td>
                                    <td>
                                        <div
                                            style='background-color:#f5f5f5;direction:ltr;padding:16px;margin-bottom:6px'>

                                        </div>
                                        <div style='border-style:solid;border-width:thin;border-color:#dadce0;border-radius:8px;padding:40px 20px'
                                            align='center' class='m_-1057103964643777674mdv2rw'><img
                                                src='/Cursus.MVC/wwwroot/images/sign_logo.png'
                                                width='74' height='24' aria-hidden='true' style='margin-bottom:16px'
                                                alt='Google' class='CToWUd' data-bit='iit'>
                                            <div
                                                style='font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;border-bottom:thin solid #dadce0;color:rgba(0,0,0,0.87);line-height:32px;padding-bottom:24px;text-align:center;word-break:break-word'>
                                                <div style='font-size:24px'>A NEW CHANGE IN YOUR ACCOUNT</div>
                                                <table align='center' style='margin-top:8px'>
                                                    <tbody>
                                                        <tr style='line-height:normal'>
                                                            <td align='right' style='padding-right:8px'></td>
                                                            <td><a
                                                                    style='font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;color:rgba(0,0,0,0.87);font-size:14px;line-height:20px'>" + username + @"</a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <div
                                                style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:14px;color:rgba(0,0,0,0.87);line-height:20px;padding-top:20px;text-align:center'>
                                                The system administrator has just changed your system information.
                                                Please log back into the system to see the changes.
                                            </div>
                                            <div
                                                style='padding-top:20px;font-size:12px;line-height:16px;color:#5f6368;letter-spacing:0.3px;text-align:center'>
                                                If you have any questions or problems, please contact with email
                                                <strong>AdminCursus@gmail.com</strong>
                                                <br>
                                                <br>
                                                <p>Best regards,</p>
                                                <p>@Cursus, 2024</p>
                                            </div>
                                        </div>
                                        <div style='text-align:left'>
                                            <div
                                                style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;color:rgba(0,0,0,0.54);font-size:11px;line-height:18px;padding-top:12px;text-align:center'>
                                                <div>You received this email to let you know about important changes to
                                                    your Curcus Account and services.</div>
                                                <div style='direction:ltr'>© 2024 Cursus, <a
                                                        class='m_-1057103964643777674afal'
                                                        style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;color:rgba(0,0,0,0.54);font-size:11px;line-height:18px;padding-top:12px;text-align:center'>Thu
                                                        Duc, Ho Chi Minh City, Viet Nam</a></div>
                                            </div>
                                        </div>
                                    </td>
                                    <td width='8' style='width:8px'></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr height='32' style='height:32px'>
                    <td></td>
                </tr>
            </tbody>
        </table>
    </div>
</body>

</html>
"; return Response;

        }
        public static string EmailNotiConfirmCourse(string username, string courseName, string status)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                        .link-confirm {{
                            display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Notification About Your Course</h1>
                        <div class='content'>
                            <h4>Notification</h4>
                            <p>Hello {username},</p>
                            <p>Your course already: {status}</p>    
                             <h4> {courseName} </h4>
                            <p>Thank you for using our service.</p>
                            <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";
            return Response;
        }
        public static string EmailNotiAccount(string username, string status)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                            .link-confirm {{
                                display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Notification About Your Account</h1>
                        <div class='content'>
                            <h4>Notification</h4>
                            <p>Hello {username},</p>
                            <p>Your account already {status} by admin</p>    
                            <p>Thank you for using our service.</p>
                            <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";
            return Response;
        }

        public static string EmailNotiConfirmInstruction(string username, string status)
        {
            string Response = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Account Updated</title>
                    <style>
                        /* Reset CSS */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                        }}
                        /* Container */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #f9f9f9;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .container h1 {{
                            text-align: center;
                            margin: 0; 
                            font-size: 32px; 
                            font-weight: 700; 
                            letter-spacing: -1px; 
                            line-height: 48px;
                        }}
                        /* Content */
                        .content {{
                            background-color: #fff;
                            padding: 20px;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        /* Footer */
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer p {{
                            color: #888;
                        }}
                        .link-confirm {{
                            display: inline-block;
                              background-color: #007bff;
                              color: white;
                              padding: 10px 20px;
                              text-align: center;
                              border-radius: 5px;
                              text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Notification About Your Regis</h1>
                        <div class='content'>
                            <h4>Notification</h4>
                            <p>Hello {username},</p>
                            <p>Your regis up to instruction is:{status}</p>    
                            <p>Thank you for using our service.</p>
                            <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                            <p>Best regards,</p>
                            <p>Cursus</p>                            
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return Response;
        }

        public static string Subscriber(string username, string instructorName)
        {
            string Response = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Account Updated</title>
                <style>
                    /* Reset CSS */
                    body, html {{
                        margin: 0;
                        padding: 0;
                        font-family: Arial, sans-serif;
                    }}
                    /* Container */
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        background-color: #f9f9f9;
                        border-radius: 10px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    /* Header */
                    .container h1 {{
                        text-align: center;
                        margin: 0; 
                        font-size: 32px; 
                        font-weight: 700; 
                        letter-spacing: -1px; 
                        line-height: 48px;
                    }}
                    /* Content */
                    .content {{
                        background-color: #fff;
                        padding: 20px;
                        border-radius: 5px;
                        margin-top: 20px;
                    }}
                    /* Footer */
                    .footer {{
                        text-align: center;
                        margin-top: 20px;
                    }}
                    .footer p {{
                        color: #888;
                    }}
                    .link-confirm {{
                        display: inline-block;
                        background-color: #007bff;
                        color: white;
                        padding: 10px 20px;
                        text-align: center;
                        border-radius: 5px;
                        text-decoration: none;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>You have new subscribers</h1>
                    <div class='content'>
                        <h4>Notification</h4>
                        <p>Hello {instructorName},</p>
                        <p>You have a new subscriber: {username}</p>    
                        <p>Thank you for using our service.</p>
                        <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                        <p>Best regards,</p>
                        <p>Cursus</p>                            
                    </div>
                    <div class='footer'>
                        <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                    </div>
                </div>
            </body>
            </html>";
            return Response;
        }
        public static string UnSubscriber()
        {
            string Response = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Account Updated</title>
                <style>
                    /* Reset CSS */
                    body, html {{
                        margin: 0;
                        padding: 0;
                        font-family: Arial, sans-serif;
                    }}
                    /* Container */
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        background-color: #f9f9f9;
                        border-radius: 10px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    /* Header */
                    .container h1 {{
                        text-align: center;
                        margin: 0; 
                        font-size: 32px; 
                        font-weight: 700; 
                        letter-spacing: -1px; 
                        line-height: 48px;
                    }}
                    /* Content */
                    .content {{
                        background-color: #fff;
                        padding: 20px;
                        border-radius: 5px;
                        margin-top: 20px;
                    }}
                    /* Footer */
                    .footer {{
                        text-align: center;
                        margin-top: 20px;
                    }}
                    .footer p {{
                        color: #888;
                    }}
                    .link-confirm {{
                        display: inline-block;
                        background-color: #007bff;
                        color: white;
                        padding: 10px 20px;
                        text-align: center;
                        border-radius: 5px;
                        text-decoration: none;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>You have new subscribers</h1>
                    <div class='content'>
                        <h4>Notification</h4>
                        <p>You have your subscriber have unsubscribe</p>    
                        <p>Thank you for using our service.</p>
                        <p>If you have any questions or problems, please contact email: datdtce171751@fpt.edu.vn.</p>
                        <p>Best regards,</p>
                        <p>Cursus</p>                            
                    </div>
                    <div class='footer'>
                        <p>&copy; 2024  datdtce171751@fpt.edu.vn. Cursus.</p>
                    </div>
                </div>
            </body>
            </html>";
            return Response;
        }
    }
}
