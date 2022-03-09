using Microsoft.AspNetCore.Http;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AdminLTE.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailAsync(
       string fromDisplayName,
       string fromEmailAddress,
       string toName,
       string toEmailAddress,
       string subject,
       string message,
       string ccname = "", string ccemail = "", params Attachment[] attachments);

        Task SendIformEmailAsync(string fromDisplayName,
        string fromEmailAddress,
        string toName,
        string toEmailAddress,
        string subject,
        string message, IFormFileCollection attachments, string ccname = "", string ccemail = "");

        Task SendEmailToSupportAsync(string subject, string message);
        Task SendExceptionEmailAsync(Exception e, HttpContext context);

        Task CreateEmailAsync(
      string fromDisplayName,
      string fromEmailAddress,
      string toName,
      string toEmailAddress,
      string subject,
      string message,
      params Attachment[] attachments);

        Task SendSMTPMeetingAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress,
              string subject, string message, string startTime, string endTime, string ccname = "", string ccemail = "", params Attachment[] attachments);

        Task SendMeetingAsync(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, string subject, string message, string startTime, string endTime, string ccname = "", string ccemail = "", params Attachment[] attachments);
        Task SendCalendarInvite(string fromDisplayName, string fromEmailAddress, string toName, string toEmailAddress, DateTime startTime, DateTime endTime, string subject, string message, string location, string ccname, string ccemail, string[] emails);
    }
}
