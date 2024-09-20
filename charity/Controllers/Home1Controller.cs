using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

public class Home1Controller : Controller
{
    //宣告了一個 _environment 變數，用來儲存當前應用程式的環境資訊（例如根目錄位置）。
    private readonly IWebHostEnvironment _environment;
    //接受並初始化 _environment
    public Home1Controller(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
   
    // GET: Index
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult testupload()
    {
        return View();
    }
    // POST: UploadFile
    //處理上傳檔案的 POST 請求方法，傳入一個 IFormFile 物件，表示使用者選擇上傳的檔案。
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        //驗證檔案是否存在
        if (file != null && file.Length > 0)
        {
            var filepath = "~/wwwroot/";
            //將wwwroot(靜態資源根目錄)與test結合
            var path = Path.Combine(filepath, "test");
            //var path = Path.Combine(_environment.WebRootPath, "test");
            // 確認 "test" 資料夾是否存在，若不存在則建立
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // 建立檔案的儲存路徑
            var filePath = Path.Combine(path, file.FileName);

            // 將檔案儲存至指定的資料夾
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ViewBag.Message = "檔案上傳成功!";
        }
        else
        {
            ViewBag.Message = "請選擇一個檔案上傳!";
        }
        //返回 Index
        return View("../Home/Index");
    }
}
