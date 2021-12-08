using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEmailService
    {
        Task<(bool isSuccess, List<string> errors)> SendEmail(string to, string subject, string content);
    }
}
