using charity.Controllers;
using charity.Models;
namespace charity.ViewModels {
    public class ProductImgViewModel {
        public Product product {  get; set; }
        public IEnumerable<ProductImg> productImgs { get; set; }
    }
}
