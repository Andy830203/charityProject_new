namespace WebAPI_for_frondEnd.DTO
{
    public class GetMemberActivityDTO
    {
        public int SignUpId { get; set; } // 報名ID

        public int ActivityId { get; set; } // 活動時段ID

        public int EventId { get; set; } // 活動ID

        public string EventName { get; set; } // 活動名稱

        public string EventDescription { get; set; } //活動描述

    }
}
