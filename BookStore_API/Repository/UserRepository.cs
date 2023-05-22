using BookStore_API.Data;
using BookStore_API.Model;
using BookStore_API.Model.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BookStore_API.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<User> _dbSet;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
           _dbSet = db.Set<User>();
        }
        public async Task CreateAsync(User entity)
        {
            await _db.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
           
            return await query.ToListAsync();
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<User> query = _dbSet;

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

        public async Task RemoveAsync(User Entity)
        {
            _dbSet.Remove(Entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<User> UpdateAsync(User Entity)
        {
            _db.Update(Entity);
            await _db.SaveChangesAsync();
            return Entity;
        }
    }
}
