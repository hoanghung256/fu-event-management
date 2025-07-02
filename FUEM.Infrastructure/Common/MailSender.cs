using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace FUEM.Infrastructure.Common
{
    public class MailSender
    {
        private readonly string smtpHost = "smtp.MailSender.com";
        private readonly int smtpPort = 587;
        private readonly string fromEmail = "fpteventmanagementsystem@MailSender.com";
        private readonly string password = "cwca unvn nsub lujp";

        private readonly string toEmail;
        private string contentType = "text/plain";
        private string subject;
        private string content;

        private Dictionary<string, string> macrosMap;

        public MailSender(IConfiguration configuration)
        {

        }

        public MailSender(params string[] toEmail)
        {
            this.toEmail = string.Join(",", toEmail);
        }

        public MailSender SetContentType(string contentType)
        {
            this.contentType = contentType;
            return this;
        }

        public MailSender SetSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public MailSender InitContent(string content)
        {
            this.content = content;
            return this;
        }

        public MailSender AppendContent(string content)
        {
            this.content += content;
            return this;
        }

        public MailSender InitMacro()
        {
            macrosMap = new Dictionary<string, string>();
            return this;
        }

        public MailSender AppendMacro(string macro, string value)
        {
            macrosMap[macro] = value;
            return this;
        }

        public async Task SendAsync()
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(fromEmail));
            foreach (var email in toEmail.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(MailboxAddress.Parse(email.Trim()));
            }

            message.Subject = subject;

            if (contentType.StartsWith("text/html"))
            {
                message.Body = new TextPart("html") { Text = content };
            }
            else
            {
                message.Body = new TextPart("plain") { Text = content };
            }

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(fromEmail, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private string InsertMacro(string str)
        {
            if (macrosMap == null) return str;

            foreach (var kv in macrosMap)
            {
                str = str.Replace($"[{kv.Key}]", kv.Value);
            }
            return str;
        }

        public async Task SendTemplateAsync(string filePath)
        {
            if (content != null || macrosMap == null) return;

            string[] lines = await System.IO.File.ReadAllLinesAsync(filePath);
            InitContent("");

            foreach (var line in lines)
            {
                AppendContent(InsertMacro(line) + "\n");
            }

            await SendAsync();
        }

        public async Task SendTemplateAsync(Uri fileUrl)
        {
            if (content != null || macrosMap == null) return;

            using var client = new System.Net.Http.HttpClient();
            var contentStr = await client.GetStringAsync(fileUrl);

            InitContent(InsertMacro(contentStr));
            await SendAsync();
        }

        public static async Task SendWithOTP(string email, string otp)
        {
            var g = new MailSender(email)
                .SetContentType("text/html; charset=UTF-8")
                .SetSubject("Verify account")
                .InitMacro()
                .AppendMacro("OTP", otp);

            await g.SendTemplateAsync(new Uri($"localhost/MailSender-template/send-otp.html"));
        }
        public void DoSomething()
        {
            int x = 5;   // Unused variable
            if (true)
            {
                Console.WriteLine("Always true branch");
            }
            else
            {
                Console.WriteLine("Dead code");
            }
        }

        public void BadMethod()
        {
            string password = "123"; // hardcoded password
            int x = 5; // unused
            if (true) { Console.WriteLine("dead code"); } else { Console.WriteLine("unreachable"); }
        }

        //public static async Task GuestRegisterEventSuccess(string email, string guestName, Event e)
        //{
        //    var g = new MailSender(email)
        //        .SetContentType("text/html; charset=UTF-8")
        //        .SetSubject("Guest Register Successfully!")
        //        .InitMacro()
        //        .AppendMacro("GuestName", guestName)
        //    .AppendMacro("EventName", e.FullName)
        //    .AppendMacro("Date", DateTimeConvertter.DateToString(e.DateOfEvent))
        //    .AppendMacro("StartTime", DateTimeConvertter.TimeToString(e.StartTime))
        //        .AppendMacro("EndTime", DateTimeConvertter.TimeToString(e.EndTime))
        //        .AppendMacro("Location", e.Location.Name);

        //}
    }
}
