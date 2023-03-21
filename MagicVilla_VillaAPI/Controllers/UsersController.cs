using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/UserAuth")]

    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected  APIResponse _apiResponse;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _apiResponse = new APIResponse();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if(loginResponse.User ==null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Username or Password is Incorrect");
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = loginResponse;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDTO model)
        {
            var isUniqueUser = _userRepository.IsUniqueUser(model.UserName);
            if(!isUniqueUser.Result)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("User already Exist");
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }
            var user = await _userRepository.Register(model);
            if(user == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Error while Registering User");
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);

            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = user;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);


        }
    }
}
