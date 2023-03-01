using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository
    {
        Task<List<VillaNumber>> GetAll(Expression<Func<VillaNumber, bool>> filter = null);
        //this is to get the single villas filter is condition like 
        //first or default etc , tracked is askNoTrack etc
        Task<VillaNumber> Get(Expression<Func<VillaNumber, bool>> filter = null, bool tracked = true);
        //it is related to the task we want to create s
        Task Create(VillaNumber entity);
        //this is for deletion 
        Task Remove(VillaNumber entity);
        //this is to save the changes that has been dones
        Task Update(VillaNumber entity);
        Task Save();
    }
}
