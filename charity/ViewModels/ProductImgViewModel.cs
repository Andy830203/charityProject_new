using charity.Controllers;
using charity.Models;
namespace charity.ViewModels {
    public class ProductImgViewModel {
        public Product product {  get; set; }
        public List<IFormFile> UploadedImages { get; set; }  // 用來上傳多張圖片
        public List<string> productImgs { get; set; }
    }
}
