namespace WebAPI_for_frondEnd.DTO {
    public class IndexProductDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SellerId { get; set; }
        public string? SellerName { get; set; }
        public int? Category { get; set; }
        public int? Price { get; set; }
        // public bool? OnShelf { get; set; }
        public DateTime? OnShelfTime { get; set; }
        public string? Description { get; set; }
        public int? Instock { get; set; }
        public string? MainImageUrl { get; set; }
        public string? CategoryName { get; set; }
    }
}
