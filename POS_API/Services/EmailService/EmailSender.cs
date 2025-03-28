using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using POS_API.Services.IServices;
using POS_API.Models.EmailModel;
using Microsoft.Extensions.Options;

namespace POS_API.Services.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(emailMessage.To));
            message.Subject = emailMessage.Subject;

            var builder = new BodyBuilder();
            if (emailMessage.IsHtml)
            {
                builder.HtmlBody = emailMessage.Body;
            }
            else
            {
                builder.TextBody = emailMessage.Body;
            }

            if (emailMessage.Attachments != null)
            {
                foreach (var attachment in emailMessage.Attachments)
                {
                    builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
                }
            }

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort,
                    _emailSettings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}