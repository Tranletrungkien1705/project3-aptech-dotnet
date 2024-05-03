using eProject3.Data;
using eProject3.DTO;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eProject3.Repository
{
    public interface IMarkOfPaintInCompRepository : IBaseRepository<MarkOfPaintInComp>
    {
        Task<List<MarkOfPaintInComp>> GetMarkAndFeedBackOfPainInCompByKeyword(string keyword);
        Task<DetailStaff> CreateDetailStaff(DetailStaff entity);

        Task<DetailStaff> UpdateDetailStaff(DetailStaff entity);

        Task<List<DetailStaff>> GetDetailStaff();

        DetailStaff GetDetailStaffById(int id);
        Task<MarkOfPaintInComp> DeleteDetailStaff(int id);
        Task<MarkOfPaintInComp> GetMarkOfPaintInComp(int id);
        Task<MarkOfPaintInComp> GetMarkByPaintInCom(int id);
    }
    public class MarkOfPaintInCompRepository : BaseRepository<MarkOfPaintInComp>, IMarkOfPaintInCompRepository
    {
        private readonly ApplicationDbContext _context;
        public MarkOfPaintInCompRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
        }

        public async Task<List<MarkOfPaintInComp>> GetMarkAndFeedBackOfPainInCompByKeyword(string keyword)
        {
            var query = from s in _context.MarkOfPaintInComps.AsQueryable()
                        where s.Mark.ToLower().Contains(keyword.ToLower()) ||
                              s.Feedback.ToLower().Contains(keyword.ToLower())
                        select s;
            return await query.ToListAsync();
        }

        public async Task<List<DetailStaff>> GetDetailStaff()
        {
            var query = from ma in _context.MarkOfPaintInComps.AsQueryable()
                        join pc in _context.PaintingInComps.AsQueryable() on ma.PaintingInCompId equals pc.Id
                        join p in _context.Paintings.AsQueryable() on pc.PaintingId equals p.Id
                        join sf in _context.Staffs.AsQueryable() on ma.TeacherId equals sf.Id
                        group ma by new
                        {
                            Id = ma.Id,
                            PaintingId = p.Id,
                            TeacherId = sf.Id,
                            Mark = ma.Mark,
                            Feedback = ma.Feedback,
                            FileEntity = p.FileEntity
                        } into grouped
                        select new DetailStaff
                        {
                            Id = grouped.Key.Id,
                            PaintingId = grouped.Key.PaintingId,
                            TeacherId = grouped.Key.TeacherId,
                            Mark = grouped.Key.Mark,
                            Feedback = grouped.Key.Feedback,
                            FileEntity = grouped.Key.FileEntity
                        };
            return await query.OrderBy(x => x.Id).ToListAsync();
        }

        public DetailStaff GetDetailStaffById(int id)
        {
            var query = from ma in _context.MarkOfPaintInComps.AsQueryable()
                        join pc in _context.PaintingInComps.AsQueryable() on ma.PaintingInCompId equals pc.Id
                        join p in _context.Paintings.AsQueryable() on pc.PaintingId equals p.Id
                        join sf in _context.Staffs.AsQueryable() on ma.TeacherId equals sf.Id
                        where ma.Id == id
                        select new DetailStaff
                        {
                            Id = ma.Id,
                            PaintingId = p.Id,
                            TeacherId = sf.Id,
                            Mark = ma.Mark,
                            Feedback = ma.Feedback,
                            FileEntity = p.FileEntity
                        };
            return query.FirstOrDefault();
        }

        public async Task<DetailStaff> CreateDetailStaff(DetailStaff entity)
        {
            if (entity != null)
            {
                var x = new MarkOfPaintInComp();
                x.Id = entity.Id;
                x.PaintingInCompId = entity.PaintingId;
                x.TeacherId = entity.TeacherId;
                x.Mark = entity.Mark;
                x.Feedback = entity.Feedback;
                await _dbSet.AddAsync(x);
                await _context.SaveChangesAsync();
                entity.Id = x.Id; // Gán giá trị Id đã được tạo tự động vào entity
                return entity;
            }
            return null;
        }

        public async Task<DetailStaff> UpdateDetailStaff(DetailStaff entity)
        {
            if (entity != null)
            {
                var x = await _dbSet.FindAsync(entity.Id);
                if (x != null)
                {
                    x.PaintingInCompId = entity.PaintingId;
                    x.TeacherId = entity.TeacherId;
                    x.Mark = entity.Mark;
                    x.Feedback = entity.Feedback;

                    if (_context.Entry(x).State != EntityState.Modified)
                    {
                        _dbSet.Update(x);
                    }

                    await _context.SaveChangesAsync();
                    return entity;
                }
            }
            return null;
        }

        public async Task<MarkOfPaintInComp> DeleteDetailStaff(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return entity; // Trả về đối tượng đã bị xóa
            }
            return null;
        }
        public async Task<MarkOfPaintInComp> GetMarkOfPaintInComp(int id)
        {
            var query = from ma in _context.MarkOfPaintInComps.AsQueryable()
                        join pc in _context.PaintingInComps.AsQueryable() on ma.PaintingInCompId equals pc.Id
                        join p in _context.Paintings.AsQueryable() on pc.PaintingId equals p.Id
                        join stu in _context.Students.AsQueryable() on p.StudentId equals stu.Id
                        where ma.Id == id
                        select ma;
            return await query.FirstOrDefaultAsync();
        }
        public async Task<MarkOfPaintInComp> GetMarkByPaintInCom(int id)
        {
            var query = from ma in _context.MarkOfPaintInComps.AsQueryable()
                        join pc in _context.PaintingInComps.AsQueryable() on ma.PaintingInCompId equals pc.Id
                        where ma.PaintingInCompId == id
                        select ma;
            return await query.FirstOrDefaultAsync();
        }

    }
}
