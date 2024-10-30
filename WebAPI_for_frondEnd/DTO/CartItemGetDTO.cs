namespace WebAPI_for_frondEnd.DTO {
    public class CartItemGetDTO {
        public int Id { get; set; }
        public int? Buyer { get; set; }
        public int? PId { get; set; }
        public int? Quantity { get; set; }
        public string ProductName { get; set; }
        public int? ProductPrice { get; set; }
        public string? MainImageUrl { get; set; }
    }
}
