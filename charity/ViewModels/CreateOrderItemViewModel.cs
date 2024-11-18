namespace charity.ViewModels {
    public class CreateOrderItemViewModel {
        public int ProductId { get; set; } // 商品 ID
        public int Quantity { get; set; } // 數量
        public decimal Price { get; set; } // 單價
    }
}
