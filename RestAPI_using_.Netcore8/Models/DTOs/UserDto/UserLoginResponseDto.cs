namespace RestAPI_using_.Netcore8.Models.DTOs.UserDto
{
    public class UserLoginResponseDto
    {
        public AppUser User { get; set; }
        public string Token { get; set; }
    }
}