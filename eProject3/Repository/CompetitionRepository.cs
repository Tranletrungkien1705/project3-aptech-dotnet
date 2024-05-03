using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace eProject3.Repository
{
    public interface ICompetitionRepository : IBaseRepository<Competition>
    {
        Task<List<Competition>> GetCompetionByKeyword(string keyword);
    }
    public class CompetitionRepository : BaseRepository<Competition>, ICompetitionRepository
    {
        public CompetitionRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            
        }

        public async Task<List<Competition>> GetCompetionByKeyword(string keyword)
        {
            var query = from st in _context.Competitions.AsQueryable()
                        where st.Name.ToLower().Contains(keyword.ToLower()) ||
                              st.Description.ToLower().Contains(keyword.ToLower()) ||
                              st.Conditions.ToLower().Contains(keyword.ToLower())
                        where st.IsDeleted == false
                        select st;

            return await query.Where(x =>
             x.CreatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email
             || x.UpdatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email).ToListAsync();
        }
    }
}
