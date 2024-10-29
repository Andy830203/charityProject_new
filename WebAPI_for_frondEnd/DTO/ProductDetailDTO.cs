namespace WebAPI_for_frondEnd.DTO {
    public class ProductDetailDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Seller { get; set; }
        public string SellerName { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
        public int? Instock { get; set; }
        public int? Category { get; set; }
        public string CategoryName { get; set; }
        public DateTime? OnShelfTime { get; set; }
        public List<String> imgUrls { get; set; }

    }
}
