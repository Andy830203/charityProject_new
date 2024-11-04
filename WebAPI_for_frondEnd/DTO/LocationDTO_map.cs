namespace WebAPI_for_frondEnd.DTO
{
    public class LocationDTO_map
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string ?Description { get; set; }
        public string ?Address { get; set; }
        public string ?PlusCode { get; set; }
        public int? Capacity { get; set; }
    }
}