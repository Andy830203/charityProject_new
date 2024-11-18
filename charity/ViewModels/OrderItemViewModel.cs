using charity.Controllers;
using charity.Models;
namespace charity.ViewModels {
    public class OrderItemViewModel {
        public int Id { get; set; }
        public int? PId { get; set; }
        public string? PName { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ShippedTime { get; set; }

        public int? Score { get; set; }
    }
}
