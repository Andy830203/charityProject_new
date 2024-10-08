using System.ComponentModel.DataAnnotations;

namespace charity.ViewModels
{
    public class EventLocViewModel
    {
        //活動地點ID
        public int Id { get; set; }
        // 活動名稱
        [Display(Name = "活動名稱")]
        public string? EventName { get; set; }
        [Display(Name = "活動人數上限")]
        public int? EventCapacity { get; set; }
        // 活動地點
        [Display(Name = "活動地點")]
        public string? LocationName { get; set; }
        [Display(Name = "活動描述")]
        public string? LocationDesc { get; set; }
        [Display(Name = "地點最大人數")]
        public int? LocCapacity { get; set; }
        // 地點順序
        [Display(Name = "地點順序")]
        public int? Order { get; set; }
    }
}
