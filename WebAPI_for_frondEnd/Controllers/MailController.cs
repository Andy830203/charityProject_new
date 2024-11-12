using CoreAPI2024;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Service;

namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendContactEmail")]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactRequestDTO model)
        {
            // 驗證前端是否傳入所有必填的資訊
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("請填寫所有欄位");
            }

            // 組合郵件內容
            string emailContent = $"姓名: {model.Name}\n" +
                                  $"電子郵件: {model.Email}\n" +
                                  $"問題內容:\n{model.Message}";

            // 發送郵件到指定信箱
            string recipientEmail = "renxuan0810@gmail.com"; // 請填寫接收聯絡我們郵件的信箱
            await _emailService.SendEmailAsync(recipientEmail,"會員意見", emailContent);

            return Ok("您的問題已成功提交，我們將盡快回覆您。");
        }
    }
}
