using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eProject3.Repository
{
    public interface IStaffRepository : IBaseRepository<Staff>
    {
        Task<List<Staff>> GetStaffsByKeyword(string keyword);
        Task<Staff> GetCurrentStaff(string User1);
    }
    public class StaffRepository : BaseRepository<Staff>, IStaffRepository
    {

        public StaffRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {

        }
        public async Task<List<Staff>> GetStaffsByKeyword(string keyword)
        {
            var query = from st in _context.Staffs.AsQueryable()
                        where st.Name.ToLower().Contains(keyword.ToLower()) ||
                              st.Email.ToLower().Contains(keyword.ToLower()) ||
                              st.Code.ToLower().Contains(keyword.ToLower()) ||
                              st.Degree.ToLower().Contains(keyword.ToLower())
                        where st.IsDeleted == false
                        select st;

            return await query.Where(x =>
            x.CreatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email
            || x.UpdatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email).ToListAsync();
        }
        public async Task<Staff> GetCurrentStaff(string User1)
        {
            var query = from s in _context.Staffs
                        where s.Email == User1
                        select s;
            return await query.FirstOrDefaultAsync();
        }
    }
}
