using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Business_Logic.Mail_Logic
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
