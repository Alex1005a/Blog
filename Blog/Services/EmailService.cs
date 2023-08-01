using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly string fromEmail = Startup.Email;
        private readonly string fromPassword = Startup.EmailPassword;
        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        public async Task SendMessageAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", fromEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 25, false);
            await client.AuthenticateAsync(fromEmail, fromPassword);
            await client.SendAsync(emailMessage);

            _logger.LogInformation($"Send messege to {toEmail}");

            await client.DisconnectAsync(true);
        }
    }
}
