using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestAPI_using_.Netcore8.Data;
using RestAPI_using_.Netcore8.Models;
using RestAPI_using_.Netcore8.Models.DTOs.UserDto;
using RestAPI_using_.Netcore8.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace RestAPI_using_.Netcore8.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly int TokenExpirationDays = 7;

        public UserRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public AppUser GetUser(string id)
        {
            return _context.AppUsers.FirstOrDefault(user => user.Id == id);
        }

        public ICollection<AppUser> GetUsers()
        {
            return _context.AppUsers.OrderBy(user => user.UserName).ToList();
        }

        public bool IsUniqueUser(string userName)
        {
            return !_context.AppUsers.Any(user => user.UserName == userName);
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.UserName.ToLower() == userLoginDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

            //user doesn't exist ?
            if (user == null || isValid == false)
            {
                return new UserLoginResponseDto { Token = "", User = null };
            }

            //User does exist
            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())

                }),
                Expires = DateTime.UtcNow.AddDays(TokenExpirationDays),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto
            {
                Token = tokenHandler.WriteToken(jwtToken),
                User = user
            };
            return userLoginResponseDto;
        }

        public async Task<UserLoginResponseDto> Register(UserRegistrationDto userRegistrationDto)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegistrationDto.UserName,
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.UserName,
                NormalizedEmail = userRegistrationDto.UserName.ToUpper(),
            };

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (result.Succeeded)
            {

                //this will run only for first time the roles are created
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("register"));
                }
            }
            await _userManager.AddToRoleAsync(user, "admin");
            var newUser = _context.AppUsers.FirstOrDefault(u => u.UserName == userRegistrationDto.UserName);

            return _mapper.Map<UserLoginResponseDto>(newUser);
        }

        private string encryptPassword(string password)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = x.ComputeHash(data);
            string response = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                response += data[i].ToString("x2").ToLower();
            }
            return response;
        }
    }
}
