using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaNumberCreateDTO villaCreateDTO);
        Task<T> UpdateAsynv<T>(VillaNumberUpdateDTO villaUpdateDto);
        Task<T> DeleteAsync<T>(int id);
    }
}
