using MagicVilla_Web.Models.Dtos;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaCreateDTO villaCreateDTO);
        Task<T> UpdateAsynv<T>(VillaUpdateDTO villaUpdateDto);
        Task<T> DeleteAsync<T>(int id);
    }
}
