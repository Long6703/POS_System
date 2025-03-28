using POS_API.Models.EmailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_API.Services.IServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
