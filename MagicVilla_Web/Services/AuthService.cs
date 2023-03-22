using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using MagicVilla_Web.Models;
using MagicVilla_Utility;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Services
{
    public class AuthService :BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _userUrl;
        public AuthService(IHttpClientFactory httpClientFactory,IConfiguration configuration):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _userUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }
        public Task<T> LoginAsync<T>(LoginRequestDTO objToCreate)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.POST,
                Url= _userUrl+ "/api/UserAuth/Login",
                Data= objToCreate

            }) ;
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO objToCreate)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.POST,
                Url = _userUrl+ "/api/UserAuth/Register",
                Data= objToCreate
            }) ;
        }
    }
}
