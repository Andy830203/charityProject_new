using System.ComponentModel.DataAnnotations;

namespace WebAPI_for_frondEnd.DTO {
    public class ProductsEditDTO {
        public string Name { get; set; }
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        // 圖片URL清單
        //public List<string> ImageUrls { get; set; }
    }
}
