// Services/IEmailService.cs
using System.Threading.Tasks;

namespace WebAPI_for_frondEnd.Service
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string toEmail, string resetLink);
    }



}
