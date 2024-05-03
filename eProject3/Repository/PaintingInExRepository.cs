using eProject3.Data;
using eProject3.Models;
using eProject3.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eProject3.Repository
{
    public interface IPaintingInExRepository : IBaseRepository<PaintingInEx>
    {
        Task<List<Painting>> GetPaintingInExByKeywordAndPrice(string keyword, float price);
        Task<List<DetailPaintingInEx>> GetDetailPaintingInEx();
        DetailPaintingInEx GetDetailPaintingInExById( int id);
        Task<DetailPaintingInEx> CreateDetailPaintingInEx(DetailPaintingInEx entity);
        Task<DetailPaintingInEx> UpdateDetailPaintingInEx(DetailPaintingInEx entity);
        Task<List<PaintingInEx>> GetPaintingInExById(int id);
        Task<List<PaintingInEx>> GetPaintingInExByExId(int id);
    }

    public class PaintingInExRepository : BaseRepository<PaintingInEx>, IPaintingInExRepository
    {
        private ApplicationDbContext _context;
        private readonly string currentUserName = "";
        public PaintingInExRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
            var currentUser = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult();
            if (currentUser != null)
            {
                this.currentUserName = currentUser.UserName;
            }
        }

        public async Task<List<Painting>> GetPaintingInExByKeywordAndPrice(string keyword, float price)
        {
            var query = from p in _context.PaintingInExs.AsQueryable()

                        join s in _context.Paintings.AsQueryable() on p.PaintingId equals s.Id

                        where s.Name.ToLower().Contains(keyword.ToLower()) ||
                              s.Description.ToLower().Contains(keyword.ToLower()) ||
                              p.Price.Equals(price) ||
                              s.Code.ToLower().Contains(keyword.ToLower())
                        select s;

            return await query.ToListAsync();
        }

        public async Task<List<DetailPaintingInEx>> GetDetailPaintingInEx()
        {
            var query = from p in _context.PaintingInExs.AsQueryable()
                        join s in _context.Paintings.AsQueryable() on p.PaintingId equals s.Id
                        join e in _context.Exhibitions.AsQueryable() on p.PaintingId equals e.Id
                        group p by new  {
                            p.Id,PaintingName= s.Id,ExhibitionName= e.Id,p.Price, p.IsSoldOut,s.FileEntity
                        } into grouped
                        select new DetailPaintingInEx{
                            Id= grouped.Key.Id,
                            PaintingName= grouped.Key.PaintingName,
                            ExhibitionName= grouped.Key.ExhibitionName,
                            Price = grouped.Key.Price,
                            IsSoldOut = grouped.Key.IsSoldOut? "da ban":"chua ban",
                            FileEntity = grouped.Key.FileEntity
                        };
            return await query.OrderBy(x=>x.Id).ToListAsync();

        }

        public DetailPaintingInEx GetDetailPaintingInExById(int id)
        {
            var x= new DetailPaintingInEx();
            var query = from p in _context.PaintingInExs.AsQueryable()
                        join s in _context.Paintings.AsQueryable() on p.PaintingId equals s.Id
                        join e in _context.Exhibitions.AsQueryable() on p.PaintingId equals e.Id
                        group p by new
                        {
                            p.Id,
                            PaintingName = s.Id,
                            ExhibitionName = e.Id,
                            p.Price,
                            p.IsSoldOut,s.FileEntity
                        } into grouped
                        where grouped.Key.Id == id
                        select new DetailPaintingInEx
                        {
                            Id = grouped.Key.Id,
                            PaintingName = grouped.Key.PaintingName,
                            ExhibitionName = grouped.Key.ExhibitionName,
                            Price = grouped.Key.Price,
                            IsSoldOut = grouped.Key.IsSoldOut==true ? "da ban" : "chua ban",
                            FileEntity = grouped.Key.FileEntity
                        };
            x = query.ToList().FirstOrDefault();
            return x;
        }


        public async Task<DetailPaintingInEx> CreateDetailPaintingInEx(DetailPaintingInEx entity)
        {
            if(entity != null)
            {
                //Console.WriteLine(entity.ToString());
                var x = new PaintingInEx();
                x.PaintingId = entity.PaintingName;
                x.ExhibitionId = entity.ExhibitionName;
                x.Price = entity.Price;
                x.IsSoldOut = entity.IsSoldOut.ToLower().Equals("ban") ? true : false;
                x.CreatedUser = currentUserName;
                x.CreatedTime= DateTime.Now;
                x.IsDeleted = false;
                await _dbSet.AddAsync(x);
                await _context.SaveChangesAsync();
                Console.WriteLine(x);
                return entity;

            }
            return null;
        }

        public async Task<DetailPaintingInEx> UpdateDetailPaintingInEx(DetailPaintingInEx entity)
        {
            if (entity != null)
            {
                var x = new PaintingInEx();
                x.Id = entity.Id;
                x.PaintingId = entity.PaintingName;
                x.ExhibitionId = entity.ExhibitionName;
                x.Price = entity.Price;
                x.IsSoldOut = entity.IsSoldOut == "ban" ? true : false;
                _dbSet.Update(x);
                await _context.SaveChangesAsync();
                return entity;

            }
            return null;
        }

        public async Task<List<PaintingInEx>> GetPaintingInExById(int id)
        {
            var query = from p in _context.PaintingInExs
                        join s in _context.Paintings on p.PaintingId equals s.Id
                        join stu in _context.Students on s.StudentId equals stu.Id
                        where p.ExhibitionId == id && stu.Email == currentUserName
                        select p;
            return await query.ToListAsync();
        }

        public async Task<List<PaintingInEx>> GetPaintingInExByExId(int id)
        {
            var query = from p in _context.PaintingInExs
                        join s in _context.Paintings on p.PaintingId equals s.Id
                        join stu in _context.Students on s.StudentId equals stu.Id
                        where p.ExhibitionId == id && p.IsDeleted ==false && s.IsDeleted== false && stu.IsDeleted== false
                        select p;
            return await query.ToListAsync();
        }


    }
}