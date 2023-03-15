using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;


namespace MagicVilla_Web.Services
{
    public class VillaNumberService: BaseService , IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClient;
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");//this will extract the url link form the appsetting
        }
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO villaNumberCreateDTO)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.POST,
                Url = villaUrl + "/api/VillaNumberApi",
                Data = villaNumberCreateDTO

            });

        }

       
        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/VillaNumberApi/" + id,


            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberApi",


            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberApi/" + id,


            });
        }

        public Task<T> UpdateAsynv<T>(VillaNumberUpdateDTO villaNumberUpdateDto)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.PUT,
                Url = villaUrl + "/api/VillaNumberApi/" + villaNumberUpdateDto.VillaNo,
                Data = villaNumberUpdateDto

            });
        }

        
    }
}


