using System.ComponentModel.DataAnnotations;

namespace charity.ViewModels
{
    public class EventLocViewModel
    {
        // 活動名稱
        [Display(Name = "活動名稱")]
        public string? EventName { get; set; }
        // 活動地點
        [Display(Name = "活動地點")]
        public string? LocationName { get; set; }
        // 地點順序
        [Display(Name = "地點順序")]
        public int? Order { get; set; }
    }
}
