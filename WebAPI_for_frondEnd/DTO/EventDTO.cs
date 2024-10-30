namespace WebAPI_for_frondEnd.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OrganizerId { get; set; }
        public string? Organizer { get; set; }
        public int? Fee { get; set; }
        public int? Capacity { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public int? CategoryId { get; set; }
        public string? Category { get; set; }
    }
}
