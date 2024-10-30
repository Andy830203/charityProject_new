namespace WebAPI_for_frondEnd.DTO {
    public class CartItemAddDTO {
        public int Id { get; set; }

        public int? BuyerId { get; set; }

        public int? SellerId { get; set; }

        public int? PId { get; set; }

        public int? Quantity { get; set; }
    }
}
