using BookStore_API.Data;
using BookStore_API.Model;
using BookStore_API.Model.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BookStore_API.Repository
{
    public class PublisherRepository : IRepository<Publisher>
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<Publisher> _dbSet;
        public PublisherRepository(ApplicationDbContext db)
        {
            _db = db;
           _dbSet = db.Set<Publisher>();
        }
        public async Task CreateAsync(Publisher entity)
        {
            await _db.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<Publisher>> GetAllAsync(Expression<Func<Publisher, bool>>? filter = null)
        {
            IQueryable<Publisher> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
           
            return await query.ToListAsync();
        }

        public async Task<Publisher> GetAsync(Expression<Func<Publisher, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Publisher> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Publisher Entity)
        {
            _dbSet.Remove(Entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<Publisher> UpdateAsync(Publisher Entity)
        {
            _db.Update(Entity);
            await _db.SaveChangesAsync();
            return Entity;
        }
    }
}
