namespace WebAPI_for_frondEnd.DTO
{
    public class EventLocationDTO
    {
        public int Id { get; set; }
        public int? LId { get; set; }
        public string? LocName { get; set; }
        public int? EId { get; set; }
        public string? belongedEvent { get; set; }
        public int? OrderInEvent { get; set; }
    }
}
