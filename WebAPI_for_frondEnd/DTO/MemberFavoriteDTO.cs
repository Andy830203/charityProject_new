namespace WebAPI_for_frondEnd.DTO
{
    public class MemberFavoriteDTO
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int enentId { get; set; }

        public bool? attendance { get; set; }
    }
}
