using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebAPI_for_frondEnd.Service;

public class EmailService : IEmailService
{
    private readonly string smtpServer;
    private readonly int smtpPort;
    private readonly string smtpUser;
    private readonly string smtpPass;

    public EmailService(IConfiguration configuration)
    {
        smtpServer = configuration["EmailSettings:SmtpServer"];
        smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
        smtpUser = configuration["EmailSettings:Username"];
        smtpPass = configuration["EmailSettings:Password"];
    }

    public async Task SendResetPasswordEmailAsync(string toEmail, string resetLink)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = "重設您的密碼",
            Body = $"請點擊以下連結以重設您的密碼：<a href='{resetLink}'>重設密碼</a>",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        using var smtpClient = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true,
        };

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = subject,
            Body = message,
            IsBodyHtml = false
        };
        mailMessage.To.Add(toEmail);

        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.Credentials = new NetworkCredential(smtpUser, smtpPass);
            client.EnableSsl = true;
            await client.SendMailAsync(mailMessage);
        }
    }
}
