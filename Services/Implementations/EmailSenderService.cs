using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using System.Net.Mail;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;
using VaaradhiPay.Services.Interfaces;



namespace VaaradhiPay.Services.Implementations
{
 
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmailSenderDTO _emailSettings;
        public EmailSenderService(ApplicationDbContext Dbcontext, IOptions<EmailSenderDTO> emailSettings)
        {
            _dbContext = Dbcontext;
            _emailSettings = emailSettings.Value;

        }

        public async Task EmailSendAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
                emailMessage.To.Add(new MailboxAddress(email, email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = htmlMessage };

                using var client = new  MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return $"data:image/jpeg;base64,{base64String}";
        }

        public async Task<string> GenerateHeader()
        {
            // Generate the image and get the URL

            try
            {
                string logo = ConvertImageToBase64("wwwroot\\MediaFiles\\Logos\\VaaradhiPay_WhiteLogo.png");
                ; // Specify the object name for header
                Console.WriteLine(logo);
                return $@"
                    <div class='header'>
                        <img src='{logo}' alt='Company Logo'/>
                    </div>
                    ";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            // Return the HTML using the image URL

        }
        public async Task<string> EmailConfirmationLetter()
        {

            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 10px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1>Email Verification</h1>
                </div>
                <div class=""card-body"">
                    <h4>Dear [User's First Name],</h4>
                    <p>Thank you for registering with us! <br>Please confirm your email address by clicking the button below:</p>

                        <br>
                    <a href=""#"" class=""btn-custom"">Click to Verify</a>
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

        public async Task<string> SignUp()
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
                margin-bottom: 10px;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 20px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1>Welcome to Vaaradhi</h1>
                </div>
                <div class=""card-body"">
                    <h4>Hi [First Name],</h4>
                    <p> 
                        Thank you for signing up with [Your Company Name]! We're thrilled to have you.</p>

                        <a href=""#"" class=""btn-custom"">Click to Login</a> <p> to get started.</p>

                       <p> If you have any questions, contact us at [support@yourcompany.com].

                        Welcome aboard!

                        Best regards,  
                        The [Your Company Name] Team</p>

                        <br>
                    
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

        public async Task<string> KycPending()
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
                margin-bottom: 10px;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 20px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1>Pending KYC Process</h1>
                </div>
                <div class=""card-body"">
                    <h4>Dear [First Name],</h4>
                    <p> 
                        We would like to kindly remind you that the KYC (Know Your Customer) process is still pending on your account. 
                        <br>
                        To proceed with the next steps, we kindly request that you complete the necessary requirements at your earliest convenience.
                        <br>
                        If you need any assistance or further information regarding the process, please feel free to reach out.
                        <br>
                        Thank you for your prompt attention to this matter.
                        <br>
                        Click here to complete KYC:
                        <br></p>
                    <a href=""#"" class=""btn-custom"">Click </a>
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

        public async Task<string> KycSuccess()
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
                margin-bottom: 10px;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 20px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1>Pending KYC Completed</h1>
                </div>
                <div class=""card-body"">
                    <h4>Dear [First Name],</h4>
                    <p> 
                        We would like to inform you that the KYC (Know Your Customer) process has been successfully completed on your behalf. All necessary steps have been finalized.
                        <br>
                        Should you require any further assistance or clarification, please do not hesitate to contact us.
                        <br>
                        We appreciate your prompt attention to this matter.
                        <br></p>
                    <a href=""#"" class=""btn-custom"">Click to Login</a>
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

        public async Task<string> WalletAdd()
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
                margin-bottom: 10px;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 20px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1>Wallet Successfully Added to Your Account</h1>
                </div>
                <div class=""card-body"">
                    <h4>Dear [First Name],</h4>
                    <p> 
                        I hope this message finds you well.
                    <br>
                    We are pleased to inform you that a wallet has been successfully added to your account. You can now begin using it for transactions and other related activities.
                    <br>
                    Should you have any questions or require further assistance, please don’t hesitate to contact us.
                    <br>
                    Thank you for your continued trust in our services.
                                            <br></p>
                    <a href=""#"" class=""btn-custom"">Click to Login</a>
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

        public async Task<string> BankAdd()
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
                margin-bottom: 10px;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 20px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1>Bank Account Successfully Added to Your Account</h1>
                </div>
                <div class=""card-body"">
                    <h4>Dear [First Name],</h4>
                    <p> 
                        I hope this message finds you well.
                        <br>
We are pleased to inform you that your bank account has been successfully added to your account.<br> You can now use it for transactions and related activities.
<br>
If you have any questions or require further assistance, please feel free to reach out to us.
<br>
Thank you for choosing Vaaradhi Web & IT Services.
                                            <br></p>
                    <a href=""#"" class=""btn-custom"">Click to Login</a>
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

        public async Task<string> UpiAdd()
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
                margin-bottom: 10px;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 20px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1>UPI Successfully Added to Your Account</h1>
                </div>
                <div class=""card-body"">
                    <h4>Dear [First Name],</h4>
                    <p> 
                       I hope this message finds you well.
<br>
We are pleased to inform you that your UPI (Unified Payments Interface) has been successfully added to your account.<br> You can now use it for seamless transactions and related activities.
<br>
If you have any questions or need further assistance, please feel free to contact us.
<br>
Thank you for your continued trust in our services.
                                            <br></p>
                    <a href=""#"" class=""btn-custom"">Click to Login</a>
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

        public async Task<string> ChangePassword()
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Email Verification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
                margin: 0;
                padding: 0;
            }}
            .header {{
                background-color: #DF0A0A;
                text-align: center;
                padding: 20px 0;
            }}
            .container {{
                display: flex;
                justify-content: center;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                padding: 20px;
            }}
            .card {{
                max-width: 600px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                overflow: hidden;
            }}
            .card-header {{
                text-align: center;
                background-color: #fff;
            }}
            .card-body {{
                padding: 20px;
                text-align: center;
                line-height: 1.6;
            }}
            .card-body h4 {{
                text-align: left;
                font-family: Poppins, Arial, sans-serif;
                margin-bottom: 10px;
            }}
            .card-body p {{
                text-align: left;
                margin-bottom: 20px;
            }}
            .btn-custom {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #DF0A0A;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
            }}
            .card-footer {{
                text-align: center;
                font-size: 12px;
                background-color: #fff;
                padding: 10px 0;
            }}
            .card-footer a {{
                text-decoration: none;
                color: #000;
            }}
        </style>
    </head>
    <body>
        <div class=""header"">
            <img src=""https://i.postimg.cc/cLKR7r1q/vaaradhipay-logo.png"" alt=""Logo"" width=""188"" height=""86.1"">
        </div>
        <div class=""container"">
            <div class=""card"">
                <div class=""card-header"">
                    <h1> Password Reset Request</h1>
                </div>
                <div class=""card-body"">
                    <h4>Dear [First Name],</h4>
                    <p> 
                       I hope this message finds you well.
<br>
We have received a request to reset the password for your account. <br>To proceed with resetting your password, please click on the link below:
                                            <br></p>
                    <a href=""#"" class=""btn-custom"">Reset Password</a>
                </div>
                <div class=""card-footer"">
                    <a href=""https://www.metrolabsservices.com"">Sriram Plaza, H.no. 1-90/2/46/1, 4th FLOOR, Vittal Rao Nagar,<br>
                    Madhapur, near Image Hospital, Hyderabad,<br>
                    Telangana, INDIA 500081</a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }
    }

}
