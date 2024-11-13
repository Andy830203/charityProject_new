using System.ComponentModel.DataAnnotations;

namespace WebAPI_for_frondEnd.DTO {
    public class ProductsCreateDTO {
        public string Name { get; set; }
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Instock { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
    }
}
