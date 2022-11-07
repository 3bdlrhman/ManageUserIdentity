using System.Net;
using System.Net.Mail;

using Microsoft.AspNetCore.Identity.UI.Services;

namespace ManageUserIdentity.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using SmtpClient client = new("smtp.gmail.com", 587);

            string From = "example@gmail.com";
            string Password = "##password##";

            var message = new MailMessage();
            message.Body = htmlMessage;
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.From = new MailAddress(From);
            message.To.Add(email);
            
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(From, Password);

            await client.SendMailAsync(message);
        }
    }
}
