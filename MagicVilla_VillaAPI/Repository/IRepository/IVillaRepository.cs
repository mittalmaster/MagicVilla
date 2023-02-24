using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        //this is to get the list of all villas
        //filter can be a condition we may be required 
        Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null);
        //this is to get the single villas filter is condition like 
        //first or default etc , tracked is askNoTrack etc
        Task<Villa> Get(Expression<Func<Villa, bool>> filter = null, bool tracked = true);
        //it is related to the task we want to create s
        Task Create(Villa entity);
        //this is for deletion 
        Task Remove(Villa entity);
        //this is to save the changes that has been dones
        Task Save();
    }
}
