using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        string MyEmail = Startup.Email;
        string MyPassword = Startup.EmailPassword;
        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        public async Task SendMessageAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", MyEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 25, false);
            await client.AuthenticateAsync(MyEmail, MyPassword);
            await client.SendAsync(emailMessage);

            _logger.LogInformation($"Sen messege to {email}");

            await client.DisconnectAsync(true);
        }
    }
}
