namespace charity.ViewModels {
    public class CreateOrderViewModel {
        public int BuyerId { get; set; } // 買家 ID
        public int? DiscountCode { get; set; } // 折扣碼（可選）
        public List<CreateOrderItemViewModel> OrderItems { get; set; } = new List<CreateOrderItemViewModel>();
    }
}
