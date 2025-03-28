using POS_API.Models.EmailModel;
using POS_API.Services.IServices;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace POS_API.Services.EmailService
{
    public class EmailServiceImp : IEmailService
    {
        private readonly Channel<EmailMessage> _emailQueue;
        public EmailServiceImp(Channel<EmailMessage> emailQueue)
        {
            _emailQueue = emailQueue;
        }

        public async Task QueueEmailAsync(EmailMessage emailMessage)
        {
            await _emailQueue.Writer.WriteAsync(emailMessage);
        }

        public async Task QueueEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            var emailMessage = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = isHtml
            };

            await QueueEmailAsync(emailMessage);
        }
    }
}