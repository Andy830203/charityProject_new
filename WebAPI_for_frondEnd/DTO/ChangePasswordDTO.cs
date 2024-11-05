namespace WebAPI_for_frondEnd.DTO
{
    public class ChangePasswordDTO
    {
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
