using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClient;
        private string villaUrl;
        public VillaService(IHttpClientFactory httpClient,IConfiguration  configuration):base(httpClient)
        {
            _httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");//this will extract the url link form the appsetting
        }
        public Task<T> CreateAsync<T>(VillaCreateDTO villaCreateDTO)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.POST,
                Url= villaUrl+ "/api/VillaAPI",
                Data = villaCreateDTO

            });
               
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/VillaAPI/"+id,


            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaAPI",
                

            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaAPI/"+id,
           

            });
        }

        public Task<T> UpdateAsynv<T>(VillaUpdateDTO villaUpdateDto)
        {
            return SendAsync<T>(new APIRequest
            {
                apiType = SD.ApiType.PUT,
                Url = villaUrl + "/api/VillaAPI/"+villaUpdateDto.Id,
                Data = villaUpdateDto

            });
        }
    }
}
