using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace Service
{
    public interface IMailService
    {
        string SimpleSend(string To, string Subject, string Body);
    }
    public class MailService : IMailService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string SMTPAddress;
        private readonly int SMTPPort;
        private readonly string SMTPUsername;
        private readonly string SMTPPassword;
        private readonly string MailboxAddress;
        private readonly string MailboxName;

        public MailService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            SMTPAddress = "serveName";
            SMTPPort = 2323;
            SMTPUsername = "username";
            SMTPPassword = "password";
            MailboxName = "prohejctName";
            MailboxAddress = "addressir";
        }

        public string SimpleSend(string To, string Subject, string Body)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(MailboxName, MailboxAddress);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(To, To);
                message.To.Add(to);

                message.Subject = Subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = Body;

                //bodyBuilder.Attachments.Add(_hostingEnvironment.WebRootPath + "\\logo.png");

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect(SMTPAddress, SMTPPort, SecureSocketOptions.None);
                client.Authenticate(SMTPUsername, SMTPPassword);

                client.Send(message);

                client.Disconnect(true);
                client.Dispose();

                return "done";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}