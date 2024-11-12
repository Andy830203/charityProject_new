namespace WebAPI_for_frondEnd.DTO
{
    public class SignUpDTO
    {
        public int Id { get; set; }
        public int? EId { get; set; }
        public int? EpId { get; set; }
        public string? periodDesc { get; set; }

        public int? Applicant { get; set; }
        public string? ApplicantName { get; set; }
    }
}
