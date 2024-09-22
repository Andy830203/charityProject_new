using System.ComponentModel.DataAnnotations;

namespace charity.ViewModels
{
    public class LocationViewModel
    {
        [Display(Name = "地點名稱")]
        public string? Name { get; set; }
        [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
        public decimal? Longitude { get; set; }
        [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
        public decimal? Latitude { get; set; }
        public string? Description { get; set; }
    }
}
