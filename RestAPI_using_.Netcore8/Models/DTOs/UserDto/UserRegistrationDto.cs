using System.ComponentModel.DataAnnotations;
namespace RestAPI_using_.Netcore8.Models.DTOs.UserDto
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Field required: Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field required: UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Field required: Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field required: Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Field required: Role")]
        public string Role { get; set; }
    }
}