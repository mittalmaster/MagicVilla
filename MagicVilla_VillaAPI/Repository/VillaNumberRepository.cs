using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task Create(VillaNumber entity)
        {
            await _db.AddAsync(entity);
            await Save();
        }

        public async Task<VillaNumber> Get(Expression<Func<VillaNumber, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<VillaNumber> query = _db.VillaNumbers;
            if (tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<VillaNumber>> GetAll(Expression<Func<VillaNumber, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<VillaNumber> query = _db.VillaNumbers;
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public async Task Remove(VillaNumber entity)
        {
            _db.VillaNumbers.Remove(entity);
            await Save();
        }
        public async Task Update(VillaNumber entity)
        {
            _db.VillaNumbers.Update(entity);
            await Save();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
        
    }
}
