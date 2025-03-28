using Microsoft.Extensions.Configuration;
using POS_API.Models.EmailModel;
using POS_API.Services.EmailService;
using POS_API.Services.IServices;
using System.Reflection;
using System.Threading.Channels;

namespace POS_API
{
    public static class Registration
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            var emailChannel = Channel.CreateUnbounded<EmailMessage>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
            services.AddSingleton(emailChannel);
            services.AddTransient<IEmailService, EmailServiceImp>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddHostedService<EmailBackgroundService>();
            return services;
        }
    }
}
