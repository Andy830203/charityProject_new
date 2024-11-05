namespace WebAPI_for_frondEnd.DTO {
    public class OrderCreateDTO {
        public int Buyer { get; set; }
        public int TotalPrice { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
