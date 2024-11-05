namespace WebAPI_for_frondEnd.DTO
{
    public class GetMemberInFoDTO
    {
        public int Id { get; set; }

        public string? ImgName { get; set; }

        public string? Nickname { get; set; }

        public required string Name { get; set; }

        public string? Birth { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public bool? Gender { get; set; }

        public string Email { get; set; }

        public string GenderDisplay { get; set; } // 新增此屬性
    }
}
