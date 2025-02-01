using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TaskManagerPet.Interfaces;

namespace TaskManagerPet.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        

        public async Task SendCode(string email, string code)
        {
            try
            {
                string? smtpServer = _configuration["EmailSettings:SmtpServer"];
                string? portString = _configuration["EmailSettings:Port"];
                string? username = _configuration["EmailSettings:Username"];
                string? password = _configuration["EmailSettings:Password"];
                string? fromEmail = _configuration["EmailSettings:From"];

                if (string.IsNullOrWhiteSpace(smtpServer) || string.IsNullOrWhiteSpace(portString) ||
                    string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(fromEmail))
                {
                    throw new InvalidOperationException("Email settings are not properly configured.");
                }

                if (!int.TryParse(portString, out int port))
                {
                    throw new FormatException("Invalid SMTP port number.");
                }

                using var smtp = new SmtpClient(smtpServer, port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(username, password)
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Your code received!",
                    Body = code,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                await smtp.SendMailAsync(mailMessage);
                Console.WriteLine("OK");
       
            }
            catch (Exception ex)
            {
                // Логируем ошибку (если используете ILogger, лучше использовать его)
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
