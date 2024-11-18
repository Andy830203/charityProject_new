namespace charity.ViewModels {
    public class OrderItemForDetailsViewModel {
        public int? ProductId { get; set; }
        public string? ProductName { get; set; } // 商品名稱
        public int? Quantity { get; set; } // 數量
        public decimal? Price { get; set; } // 單價
        public decimal? Subtotal { get; set; } // 小計
    }
}
