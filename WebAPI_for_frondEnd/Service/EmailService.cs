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
        smtpServer = configuration["SmtpSettings:Server"];
        smtpPort = int.Parse(configuration["SmtpSettings:Port"]);
        smtpUser = configuration["SmtpSettings:User"];
        smtpPass = configuration["SmtpSettings:Password"];
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
}
