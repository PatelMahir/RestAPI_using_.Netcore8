using Microsoft.AspNetCore.Identity;
namespace RestAPI_using_.Netcore8.Models
{
    public class AppUser:IdentityUser
    {
        public string Name {  get; set; }
        public string Email {  get; set; }
    }
}