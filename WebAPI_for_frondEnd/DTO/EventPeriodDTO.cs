namespace WebAPI_for_frondEnd.DTO
{
    public class EventPeriodDTO
    {
        public int Id { get; set; }
        public int? EId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
    }
}
