using System.ComponentModel.DataAnnotations;
namespace RestAPI_using_.Netcore8.Models.DTOs.UserDto
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Field required: UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Field required: Password")]
        public string Password { get; set; }
    }
}