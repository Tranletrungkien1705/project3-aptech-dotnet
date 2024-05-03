using eProject3.Data;
using eProject3.Models;
using eProject3.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace eProject3.Repository
{
    public interface IPaintingInCompRepository : IBaseRepository<PaintingInComp>
    {
        Task<List<DetailPaintingInComp>> GetDetailPaintingInComp();
        DetailPaintingInComp GetDetailPaintingInCompById(int id);
        Task<DetailPaintingInComp> CreateDetailPaintingInComp(DetailPaintingInComp entity);
        Task<DetailPaintingInComp> UpdateDetailPaintingInComp(DetailPaintingInComp entity);
        Task<DetailPaintingInComp> DeleteDetailPaintingInCompAsync(int id);
        Task<List<PaintingInComp>> GetPaintingInCompById(int id);
        Task<List<PaintingInComp>> GetPaintingInCompByCompId(int id);
        String GetCurrentUser();
    }

    public class PaintingInCompRepository : BaseRepository<PaintingInComp>, IPaintingInCompRepository
    {
        private ApplicationDbContext _context;
        private readonly string currentUserName = "";
        public PaintingInCompRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
            var currentUser = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult();
            if (currentUser != null)
            {
                this.currentUserName = currentUser.UserName;
            }
        }



        public async Task<List<DetailPaintingInComp>> GetDetailPaintingInComp()
        {
            var query = from p in _context.PaintingInComps.AsQueryable()
                        join s in _context.Paintings.AsQueryable() on p.PaintingId equals s.Id
                        join c in _context.Competitions.AsQueryable() on p.CompetitionId equals c.Id
                        group p by new
                        {
                            p.Id,
                            PaintingName = s.Id,
                            CompetitionName = c.Id,
                            DateOfPost = p.DateOfPost,
                            s.FileEntity
                        } into grouped
                        select new DetailPaintingInComp
                        {
                            Id = grouped.Key.Id,
                            PaintingName = grouped.Key.PaintingName,
                            CompetitionName = grouped.Key.CompetitionName,
                            DateOfPost = grouped.Key.DateOfPost,
                            FileEntity = grouped.Key.FileEntity
                        };
            return await query.OrderBy(x => x.Id).ToListAsync();
        }

        public DetailPaintingInComp GetDetailPaintingInCompById(int id)
        {
            var query = from p in _context.PaintingInComps
                        join s in _context.Paintings on p.PaintingId equals s.Id
                        join c in _context.Competitions on p.CompetitionId equals c.Id
                        where p.Id == id
                        select new DetailPaintingInComp
                        {
                            Id = p.Id,
                            PaintingName = s.Id,
                            CompetitionName = c.Id,
                            DateOfPost = p.DateOfPost,
                            FileEntity = s.FileEntity
                        };
            return query.FirstOrDefault();
        }

        public async Task<DetailPaintingInComp> CreateDetailPaintingInComp(DetailPaintingInComp entity)
        {
            if (entity != null)
            {
                var x = new PaintingInComp();

                x.PaintingId = entity.PaintingName;
                x.CompetitionId = entity.CompetitionName;
                x.DateOfPost = entity.DateOfPost;
                x.CreatedUser = currentUserName;
                x.CreatedTime = DateTime.Now;
                x.IsDeleted = false;
                await _dbSet.AddAsync(x);
                await _context.SaveChangesAsync();
                entity.Id = x.Id; // Gán giá trị Id đã được tạo tự động vào entity
                return entity;
            }
            return null;
        }

        public async Task<DetailPaintingInComp> UpdateDetailPaintingInComp(DetailPaintingInComp entity)
        {
            if (entity != null)
            {
                var x = await _dbSet.FindAsync(entity.Id);
                if (x != null)
                {
                    x.PaintingId = entity.PaintingName;
                    x.CompetitionId = entity.CompetitionName;
                    x.DateOfPost = entity.DateOfPost;
                    _dbSet.Update(x);
                    await _context.SaveChangesAsync();
                    return entity;
                }
            }
            return null;
        }

        public async Task<DetailPaintingInComp> DeleteDetailPaintingInCompAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

            }
            return null;
        }


        public async Task<List<PaintingInComp>> GetPaintingInCompById(int id)
        {
            var query = from p in _context.PaintingInComps
                        join s in _context.Paintings on p.PaintingId equals s.Id
                        join stu in _context.Students on s.StudentId equals stu.Id
                        where p.CompetitionId == id && stu.Email == currentUserName
                        select p;
            return await query.ToListAsync();
        }
        public async Task<List<PaintingInComp>> GetPaintingInCompByCompId(int id)
        {
            var query = from p in _context.PaintingInComps
                        join s in _context.Paintings on p.PaintingId equals s.Id
                        join stu in _context.Students on s.StudentId equals stu.Id
                        where p.CompetitionId == id 
                        select p;
            return await query.ToListAsync();
        }
        public String GetCurrentUser()
        {
            var query = currentUserName;
            return  query;
        }
    }
}