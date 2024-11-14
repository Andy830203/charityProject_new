// Services/IEmailService.cs
using System.Threading.Tasks;

namespace WebAPI_for_frondEnd.Service
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string toEmail, string resetLink);

        // 新增一個通用的發送郵件方法
        Task SendEmailAsync(string toEmail, string subject, string message);
    }



}
