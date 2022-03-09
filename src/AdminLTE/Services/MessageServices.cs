using AdminLTE.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ConfigurationManager = AdminLTE.Common.Extensions.ConfigurationManager;

namespace AdminLTE.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
        
        public Task SendCalendarInvite(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, DateTime startTime, DateTime endTime, string subject, string message, string location, string ccname, string ccemail, string[] emails)
        {

            MailMessage msg = new MailMessage();
            //Now we have to set the value to Mail message properties

            //Note Please change it to correct mail-id to use this in your application
            msg.From = new MailAddress(fromEmailAddress, fromDisplayName);

            foreach (var item in emails)
            {
                msg.To.Add(new MailAddress(item));
            }

            msg.To.Add(new MailAddress(toEmailAddress, toName));
            msg.CC.Add(new MailAddress(ccemail, ccname));// it is optional, only if required
            msg.Subject = subject;
            msg.Body = message;
            // msg.Priority = "";

            // Now Contruct the ICS file using string builder
            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Schedule a Meeting");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("METHOD:REQUEST");
            str.AppendLine("BEGIN:VEVENT");
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime));
            str.AppendLine("LOCATION: " + location);
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            //Now sending a mail with attachment ICS file.  
            string smtpserver = ConfigurationManager.AppSetting["EmailConfiguration:SmtpServer"];
            string smtpusername = ConfigurationManager.AppSetting["EmailConfiguration:SmtpUsername"];
            string smtppassword = ConfigurationManager.AppSetting["EmailConfiguration:SmtpPassword"];

            SmtpClient client = new SmtpClient(smtpserver);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(smtpusername, smtppassword);

            msg.Headers.Add("Content-class", "urn:content-classes:calendarmessage");

            //Convert Alternate View
            var contype = new System.Net.Mime.ContentType("text/calendar");
            contype.Parameters.Add("method", "REQUEST");
            contype.Parameters.Add("name", "Meeting.ics");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);
            msg.AlternateViews.Add(avCal);

            client.Send(msg);
            return Task.FromResult(0);
        }

        public Task SendExceptionEmailAsync(Exception e, HttpContext context)
        {
            return Task.FromResult(0);
        }

        public Task SendMeetingAsync(string number, string message)
        {
            return Task.FromResult(0);
        }

        Task IEmailSender.SendSMTPMeetingAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string message, string startTime, string endTime, string ccname, string ccemail, params Attachment[] attachments)
        {
            return Task.FromResult(0);
        }

        Task IEmailSender.SendMeetingAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string message, string startTime, string endTime, string ccname, string ccemail, params Attachment[] attachments)
        {
            return Task.FromResult(0);
        }

 

        public class Meeting
        {
            public string fromDisplayName { get; set; }
            public string fromEmailAddress { get; set; }
            public string toName { get; set; }
            public string toEmailAddress { get; set; }
            public string subject { get; set; }
            public string message { get; set; }
            public string location { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public Attachment[] Attachment { get; set; }

        }



        private readonly IConfiguration _configuration;


        public AuthMessageSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress,
            string subject, string message,  string ccname = null, string ccemail = null, params Attachment[] attachments)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromDisplayName, fromEmailAddress));
            email.To.Add(new MailboxAddress(toName, toEmailAddress));
            email.Subject = subject;

            if (ccemail != null)
            {
            email.Cc.Add(new MailboxAddress(ccname, ccemail));// it is optional, only if required
            }

            var body = new BodyBuilder
            {
                HtmlBody = message
            };

            foreach (var attachment in attachments)
            {
                using (var stream = await attachment.ContentToStreamAsync())
                {
                    body.Attachments.Add(attachment.FileName, stream);
                }
            }

            email.Body = body.ToMessageBody();


            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                string smtpserver = ConfigurationManager.AppSetting["EmailConfiguration:SmtpServer"];
                string smtpusername = ConfigurationManager.AppSetting["EmailConfiguration:SmtpUsername"];
                string smtppassword = ConfigurationManager.AppSetting["EmailConfiguration:SmtpPassword"];

                // Start of provider specific settings
                await client.ConnectAsync(smtpserver, 587, false).ConfigureAwait(false);
                await client.AuthenticateAsync(smtpusername, smtppassword).ConfigureAwait(false);
                // End of provider specific settings

                await client.SendAsync(email).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

            //  return Task.FromResult(0);
        }

        public async Task CreateEmailAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress,
      string subject, string message,  string ccname = null, string ccemail = null, params Attachment[] attachments)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromDisplayName, fromEmailAddress));
            email.To.Add(new MailboxAddress(toName, toEmailAddress));
            email.Subject = subject;
            if (ccemail != null)
            {
                email.Cc.Add(new MailboxAddress(ccname, ccemail));// it is optional, only if required
            }

            var body = new BodyBuilder
            {
                HtmlBody = message
            };

            foreach (var attachment in attachments)
            {
                using (var stream = await attachment.ContentToStreamAsync())
                {
                    body.Attachments.Add(attachment.FileName, stream);
                }
            }

            email.Body = body.ToMessageBody();


        }

        public async Task SendEmailToSupportAsync(string subject, string message)
        {
            await SendEmailAsync("HRDB Error", "Recuitment@localsolutions.org.uk", "John OShaughnessy", "joshaughnessy@localsolutions.org.uk", subject
               , message);
        }

        //public async Task SendExceptionEmailAsync(Exception e, HttpContext context)
        //{
        //    var message = _viewRenderer.Render("Common/Emails/ExceptionEmail", new ExceptionEmailModel(e, context));
        //    await SendEmailToSupportAsync("Exception", message);
        //}


        public async void SendOutEmail(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string message, string ccname = "", string ccemail = "", params Attachment[] attachments)
        {
            await SendEmailAsync(fromDisplayName, fromEmailAddress, toName, toEmailAddress, subject, message, ccname, ccemail, attachments);

        }

        public async Task SendIformEmailAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string message, IFormFileCollection attachments, string ccname = null, string ccemail = null)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromDisplayName, fromEmailAddress));
            email.To.Add(new MailboxAddress(toName, toEmailAddress));
            email.Subject = subject;
            if (ccemail != null)
            {
                email.Cc.Add(new MailboxAddress(ccname, ccemail));// it is optional, only if required
            }

            var body = new BodyBuilder();
            body.HtmlBody = message;


            foreach (var attachment in attachments)
            {
                if (attachment.Length > 0)
                {
                    string fileName = Path.GetFileName(attachment.FileName);
                    body.Attachments.Add(fileName, attachment.OpenReadStream());
                }
            }
            email.Body = body.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");


                string smtpserver = ConfigurationManager.AppSetting["EmailConfiguration:SmtpServer"];
                string smtpusername = ConfigurationManager.AppSetting["EmailConfiguration:SmtpUsername"];
                string smtppassword = ConfigurationManager.AppSetting["EmailConfiguration:SmtpPassword"];

                // Start of provider specific settings
                await client.ConnectAsync(smtpserver, 587, false).ConfigureAwait(false);
                await client.AuthenticateAsync(smtpusername, smtppassword).ConfigureAwait(false);
                // End of provider specific settings

                await client.SendAsync(email).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

            //  return Task.FromResult(0);
        }
        //public void SendCalendarInvite(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, DateTime startTime, DateTime endTime, string subject, string message, string location, string ccname, string ccemail)
        //{

        //    MailMessage msg = new MailMessage();
        //    //Now we have to set the value to Mail message properties

        //    //Note Please change it to correct mail-id to use this in your application
        //    msg.From = new MailAddress(fromEmailAddress, fromDisplayName);
        //    msg.To.Add(new MailAddress(toEmailAddress, toName));
        //    msg.CC.Add(new MailAddress(ccemail, ccname));// it is optional, only if required
        //    msg.Subject = subject;
        //    msg.Body = message;
        //    // msg.Priority = "";

        //    // Now Contruct the ICS file using string builder
        //    StringBuilder str = new StringBuilder();
        //    str.AppendLine("BEGIN:VCALENDAR");
        //    str.AppendLine("PRODID:-//Schedule a Meeting");
        //    str.AppendLine("VERSION:2.0");
        //    str.AppendLine("METHOD:REQUEST");
        //    str.AppendLine("BEGIN:VEVENT");
        //    str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime));
        //    str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
        //    str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime));
        //    str.AppendLine("LOCATION: " + location);
        //    str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
        //    str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
        //    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
        //    str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
        //    str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

        //    str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

        //    str.AppendLine("BEGIN:VALARM");
        //    str.AppendLine("TRIGGER:-PT15M");
        //    str.AppendLine("ACTION:DISPLAY");
        //    str.AppendLine("DESCRIPTION:Reminder");
        //    str.AppendLine("END:VALARM");
        //    str.AppendLine("END:VEVENT");
        //    str.AppendLine("END:VCALENDAR");

        //    //Now sending a mail with attachment ICS file.  
        //    string smtpserver = ConfigurationManager.AppSetting["EmailConfiguration:SmtpServer"];
        //    string smtpusername = ConfigurationManager.AppSetting["EmailConfiguration:SmtpUsername"];
        //    string smtppassword = ConfigurationManager.AppSetting["EmailConfiguration:SmtpPassword"];

        //    SmtpClient client = new SmtpClient(smtpserver);
        //    client.EnableSsl = true;
        //    client.Credentials = new NetworkCredential(smtpusername, smtppassword);

        //    msg.Headers.Add("Content-class", "urn:content-classes:calendarmessage");

        //    //Convert Alternate View
        //    var contype = new System.Net.Mime.ContentType("text/calendar");
        //    contype.Parameters.Add("method", "REQUEST");
        //    contype.Parameters.Add("name", "Meeting.ics");
        //    AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);
        //    msg.AlternateViews.Add(avCal);

        //    client.Send(msg);
        //}

      
        Task IEmailSender.CreateEmailAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string message, params Attachment[] attachments)
        {
            return Task.FromResult(0);
        }
    }
}
