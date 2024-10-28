namespace WebAPI_for_frondEnd.DTO {
    public class ProductQueryDTO {
        public string Keyword { get; set; }
        public int CategoryId { get; set; }
        public int Page { get; set; }
        public string SortBy { get; set; }
        public int PriceThreshold { get; set; }
    }
}
