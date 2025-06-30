namespace WarApi.Models.DTO
{
    public class LoginResponseDto
    {
        public Player User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
