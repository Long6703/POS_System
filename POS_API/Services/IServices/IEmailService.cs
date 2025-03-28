using POS_API.Models.EmailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_API.Services.IServices
{
    public interface IEmailService
    {
        Task QueueEmailAsync(EmailMessage emailMessage);
        Task QueueEmailAsync(string to, string subject, string body, bool isHtml = true);
    }
}
