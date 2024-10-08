using charity.Models;
using System.ComponentModel.DataAnnotations;

namespace charity.ViewModels
{
    public class EventsViewModel
    {
        //少組織者與'優先級'
        /*
         * 類別 活動名稱 報名人數 活動地點 活動時間 報名費 活動人數 照片 (更多)
         */
        // 事件ID
        public int Id { get; set; }
        // 優先級
        [Display(Name = "優先級")]
        public int? Priority { get; set; }
        // 類別
        [Display(Name = "類別")]
        public string? Category { get; set; }
        // 活動名稱
        [Display(Name = "活動名稱")]
        public string? Name { get; set; }
        // 報名人數
        [Display(Name = "報名人數")]
        public int? Count { get; set; }
        // 活動地點
        [Display(Name = "活動地點")]
        public EventLocation? Location { get; set; }
        // 活動時間
        // 名稱
        [Display(Name = "時段名稱")]
        public string? PeriodDesc { get; set; }
        // 時段
        [Display(Name = "活動時段")]
        public string? Period { get; set; }
        // 報名費
        [Display(Name = "報名費")]
        public int? Fee { get; set; }
        // 報名名額
        [Display(Name = "報名名額")]
        public int? Capacity { get; set; }
        // 照片
        [Display(Name = "照片名稱")]
        public string? ImageName { get; set; }
    }
}
