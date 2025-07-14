using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Reflection;

namespace FUEM.Infrastructure.Common.MailSender
{
    enum EmailTemplate
    {
        SendOTP,
        EventApprovalResult,   // Send to event's creator the approval result of their event
        GuestRegisterSuccess,   // Send student when they register event as guest successfully
        NewPendingEvent,    // Send to admin when a new event is created and waiting for admin's approval
    }

    public class MailSender
    {
        private readonly string smtpHost = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string fromEmail = "fpteventmanagementsystem@gmail.com";
        private readonly string password = "cwca unvn nsub lujp";
        //private readonly string fromEmail = "hunghvde180038@fpt.edu.vn";
        //private readonly string password = "tkrs dyhs edzf ukmu"; // for hunghvde180038@fpt.edu.vn

        private readonly string toEmail;
        private string contentType = "text/plain";
        private string subject;
        private string content;

        private static readonly Dictionary<EmailTemplate, string> templatePaths = new Dictionary<EmailTemplate, string>
        {
            { EmailTemplate.SendOTP, "send-otp.html" },
            { EmailTemplate.EventApprovalResult, "event-approval-result.html" },
            { EmailTemplate.GuestRegisterSuccess, "guest-register-success.html" },
            { EmailTemplate.NewPendingEvent, "new-pending-event.html" },
        };

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

            using var client = new HttpClient();
            var contentStr = await client.GetStringAsync(fileUrl);

            InitContent(InsertMacro(contentStr));
            await SendAsync();
        }

        public static async Task SendOTPAsync(string email, string otp)
        {
            var g = new MailSender(email)
                .SetContentType("text/html; charset=UTF-8")
                .SetSubject("Verify account!")
                .InitMacro()
                .AppendMacro("OTP", otp);

            await g.SendTemplateAsync(GetTemplateActualPath(EmailTemplate.SendOTP));
        }

        private static string GetTemplateActualPath(EmailTemplate template)
        {
            string templateName;
            templatePaths.TryGetValue(template, out templateName);
            
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(assemblyPath, "Common", "MailSender", "Templates", templateName);
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
