using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly IlogService _ilog;

        public EmailService(IlogService ilog)
        {
            _ilog = ilog;
        }
        public async Task<(bool isSuccess, List<string> errors)> SendEmail(string to, string subject, string content)
        {
            var err = new List<string>();


            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress(PublicHelper.EmailAddress);
                mail.Subject = subject;
                mail.Body = content;
                mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(PublicHelper.EmailAddress, PublicHelper.EmailPass);
                await smtp.SendMailAsync(mail);
                return (true, null);

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendEmail", "Email");
                err.Add("مشکلی رخ داده است");
                return (false, err);
            }
        }
    }
}
