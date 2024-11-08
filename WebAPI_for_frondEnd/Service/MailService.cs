using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace WebAPI_for_frondEnd.Service
{
    public class MailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Charity_light years官方", _configuration["EmailSettings:SenderEmail"]));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
