using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eProject3.Repository
{
    public interface IAwardRepository : IBaseRepository<Award>
    {
        Task<List<Award>> GetAwardsWhichBeingReceivedByStudentInCompetitionByKeyword(string keyword);
        Task<List<Award>> GetAwardInExactCompetition();
    }
    public class AwardRepository : BaseRepository<Award>, IAwardRepository
    {
        private readonly ApplicationDbContext _context;
        public AwardRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
        }

        public async Task<List<Award>> GetAwardsWhichBeingReceivedByStudentInCompetitionByKeyword(string keyword)
        {
            var query = from aw in _context.Awards.AsQueryable()
                        join p in _context.PaintingInComps.AsQueryable() on aw.PaintingInCompId equals p.Id
                        join pa in _context.Paintings.AsQueryable() on p.Id equals pa.Id  
                        join st in _context.Students.AsQueryable() on pa.StudentId equals st.Id
                        where aw.Name.ToLower().Contains(keyword.ToLower()) ||
                              aw.RemarksOfCompetion.ToLower().Contains(keyword.ToLower())
                        select aw;

            return await query.ToListAsync();
        }

        public async Task<List<Award>> GetAwardInExactCompetition()
        {
            var query = from aw in _context.Awards.AsQueryable()
                        join pc in _context.PaintingInComps.AsQueryable() on aw.PaintingInCompId equals pc.Id
                        join c in _context.Competitions.AsQueryable() on pc.CompetitionId equals c.Id
                        where aw.IsDeleted == false && pc.IsDeleted == false && c.IsDeleted == false
                        select aw;
            return await query.Where(x =>
            x.CreatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email
            || x.UpdatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email).ToListAsync();
        }
    }
}
