namespace WebAPI_for_frondEnd.DTO {
    public class UserGetOwnOrderDTO {
        public int Id { get; set; }

        public int? Buyer { get; set; }

        public int? TotalPrice { get; set; }

        public string Status { get; set; }

        public DateTime? OrderTime { get; set; }
    }
}
