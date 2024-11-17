using charity.Controllers;
using charity.Models;
namespace charity.ViewModels {
    public class OrderViewModel {
        public int Id { get; set; }
        public int? Buyer { get; set; }
        public int? TotalPrice { get; set; }
        public int? Status { get; set; }
        public string? StatusName { get; set; }
        public DateTime? OrderTime { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public OrderViewModel() {
            OrderItems = new List<OrderItemViewModel>();
        }
    }
}
