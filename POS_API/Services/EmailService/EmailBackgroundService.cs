using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using POS_API.Models.EmailModel;
using POS_API.Services.IServices;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace POS_API.Services.EmailService
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly Channel<EmailMessage> _emailQueue;
        private readonly IServiceProvider _serviceProvider;

        public EmailBackgroundService(
            Channel<EmailMessage> emailQueue,
            IServiceProvider serviceProvider)
        {
            _emailQueue = emailQueue;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await foreach (var emailMessage in _emailQueue.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                    await emailSender.SendEmailAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while sending email: {ex.Message}");
                }
            }

        }
    }
}