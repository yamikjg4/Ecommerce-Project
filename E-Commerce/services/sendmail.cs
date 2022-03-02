using E_Commerce.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.services
{
    public class sendmail : Isendmail
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public async Task SendTestEmail(UserEmail userEmail)
        {
            userEmail.Subject = UpdatePlaceHolders("Hello {{UserName}}, This is test email subject from book store web app", userEmail.PlaceHolders);

            userEmail.Body = UpdatePlaceHolders(GetEmailBody("TestEmail"), userEmail.PlaceHolders);

            await SendEmail(userEmail);
        }

        public async Task SendEmailConfirmationEmail(UserEmail userEmail)
        {
            userEmail.Subject = UpdatePlaceHolders("Hello {{UserName}}, Confirm your email id.", userEmail.PlaceHolders);

            userEmail.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmail.PlaceHolders);

            await SendEmail(userEmail);
        }

        public async Task SendEmailForForgotPassword(UserEmail userEmail)
        {
            userEmail.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password.", userEmail.PlaceHolders);

            userEmail.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmail.PlaceHolders);

            await SendEmail(userEmail);
        }

        public sendmail(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        private async Task SendEmail(UserEmail userEmail)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmail.Subject,
                Body = userEmail.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };

            foreach (var toEmail in userEmail.ToEmails)
            {
                mail.To.Add(toEmail);
            }


            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
