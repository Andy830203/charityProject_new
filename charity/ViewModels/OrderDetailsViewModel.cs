using charity.Controllers;
using charity.Models;
namespace charity.ViewModels {
    public class OrderDetailsViewModel {
        public int OrderId { get; set; } // 訂單 ID
        public int? BuyerId { get; set; }
        public string? BuyerName { get; set; } // 購買者名稱
        public decimal? TotalPrice { get; set; } // 總價
        public string Status { get; set; } // 訂單狀態
        public DateTime? OrderTime { get; set; } // 下單時間
        public List<OrderItemForDetailsViewModel> Items { get; set; } = new List<OrderItemForDetailsViewModel>(); // 訂單細項列表
    }
}
