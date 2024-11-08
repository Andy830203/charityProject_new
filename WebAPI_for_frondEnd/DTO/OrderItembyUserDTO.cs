namespace WebAPI_for_frondEnd.DTO {
    public class OrderItembyUserDTO {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? UnitPrice { get; set; }
        public int? Seller { get; set; }
        public string SellerName { get; set; }
        public DateTime? ShippedTime { get; set; }
    }
}
