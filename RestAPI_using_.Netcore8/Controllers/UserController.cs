using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI_using_.Netcore8.Models.DTOs;
using RestAPI_using_.Netcore8.Models.DTOs.CategoryDto;
using RestAPI_using_.Netcore8.Models.DTOs.UserDto;
using RestAPI_using_.Netcore8.Repository.IRepository;
using System.Net;

namespace RestAPI_using_.Netcore8.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _responseApi;
        public UserController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _responseApi = new ResponseApi();
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsers()
        {
            var userList = _userRepository.GetUsers();
            var userListDto = new List<UserDto>();
            foreach (var user in userList)
            {
                userListDto.Add(_mapper.Map<UserDto>(user));
            }
            return Ok(userListDto);
        }
        [Authorize(Roles ="admin")]
        [HttpGet("{userId:int}",Name ="GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Getuser(string userId)
        {
            var user=_userRepository.GetUser(userId);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CategoryDto>(user));
        }
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            if (!_userRepository.IsUniqueUser(userRegistrationDto.UserName))
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("Username already exists");
                return BadRequest();
            }

            var newUser = await _userRepository.Register(userRegistrationDto);
            if (newUser == null)
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("Error registering the user");
                return BadRequest();
            }

            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            return Ok(_responseApi);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var responseLogin = await _userRepository.Login(userLoginDto);

            if (responseLogin.User == null || string.IsNullOrEmpty(responseLogin.Token))
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("Incorrect user and password");
                return BadRequest(_responseApi);
            }
            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            _responseApi.Result = responseLogin;
            return Ok(_responseApi);
        }
    }
}