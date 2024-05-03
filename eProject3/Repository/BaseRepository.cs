using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eProject3.Repository
{
    public interface IBaseRepository<T> where T : Base
    {

        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllPublicAsync();
        Task<List<T>> FilterAsync( Expression<Func<T, bool>> filter = null);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);

    }
    public class BaseRepository<T> : IBaseRepository<T> where T : Base
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly UserManager<CustomUser> _userManager;
        protected readonly string currentUserId = "";
        protected readonly IHttpContextAccessor _contextAccessor;


        public BaseRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            var currentUser = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult();
            if (currentUser != null)
            {
                this.currentUserId = currentUser.UserName;
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            var id = entity.Id;
            if (entity != null)
            {
                entity.CreatedUser = currentUserId;
                entity.CreatedTime = DateTime.Now;
                entity.IsDeleted = false;
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            if (entity != null)
            {
                entity.DeletedUser = currentUserId;
                entity.IsDeleted = true;
                entity.DeletedTime = DateTime.Now;
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter = null)
        {
            var dataRows = _dbSet.AsQueryable();

            if (filter != null)
            {
                dataRows = dataRows.Where(filter);
            }
            var result = await dataRows.ToListAsync();
            return result;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _dbSet.Where(x => 
            (x.IsDeleted == false && x.CreatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email)
            || (x.IsDeleted == false && x.UpdatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email)).ToListAsync();
            //var result = await _dbSet.ToListAsync();
            return result;
        }
        public async Task<List<T>> GetAllPublicAsync()
        {
            var result = await _dbSet.Where(x => x.IsDeleted == false).ToListAsync();
            //var result = await _dbSet.ToListAsync();
            return result;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity != null)
            {
                _dbSet.Update(entity);
                entity.UpdatedUser = currentUserId;
                entity.UpdatedTime = DateTime.Now;
                entity.IsDeleted = false;
                await _context.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            if (id > 0)
            {
                var result = await _dbSet.FindAsync(id);
                return result;
            }
            return null;
        }

    }
}
