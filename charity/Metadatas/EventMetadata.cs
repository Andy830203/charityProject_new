using System.ComponentModel.DataAnnotations;

namespace charity.Models
{
    public class EventMetadata
    {
        [Display(Name = "活動ID")]
        public int Id { get; set; }

        [Display(Name = "活動名稱")]
        public string? Name { get; set; }

        public int? OrganizerId { get; set; }

        [Display(Name = "報名費")]
        public int? Fee { get; set; }

        [Display(Name = "報名名額")]
        public int? Capacity { get; set; }

        [Display(Name = "活動描述")]
        public string? Description { get; set; }

        [Display(Name = "急迫性")]
        public int? Priority { get; set; }

        public int? CategoryId { get; set; }

        [Display(Name = "活動種類")]
        public virtual EventCategory? Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<EventImg> EventImgs { get; set; } = new List<EventImg>();

        public virtual ICollection<EventLocation> EventLocations { get; set; } = new List<EventLocation>();

        public virtual ICollection<EventPeriod> EventPeriods { get; set; } = new List<EventPeriod>();

        [Display(Name = "發起人")]
        public virtual Member? Organizer { get; set; }
    }
}