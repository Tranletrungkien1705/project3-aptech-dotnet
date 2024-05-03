using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eProject3.Repository
{
    public interface IAdminRepository : IBaseRepository<Administrator>
    {

        Task<List<Staff>> GetStaffByKeyword(string keyword);
        Task<List<Student>> GetStudentByKeyword(string keyword);
        Task<List<CustomUser>> GetAllCustomUser();
    }
    public class AdminRepository : BaseRepository<Administrator>, IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
        }

        public async Task<List<Staff>> GetStaffByKeyword(string keyword)
        {
            var query = from st in _context.Staffs.AsQueryable()
                        where st.Name.ToLower().Contains(keyword.ToLower()) ||
                              st.Code.ToLower().Contains(keyword.ToLower()) ||
                              st.Email.ToLower().Contains(keyword.ToLower())

                        where st.IsDeleted == false                                 
                        select st;

            return await query.Where(x =>
             x.CreatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email
             || x.UpdatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email).ToListAsync();
        }

        public async Task<List<Student>> GetStudentByKeyword(string keyword)
        {
            var query = from s in _context.Students.AsQueryable()
                        where s.Name.ToLower().Contains(keyword.ToLower()) ||
                              s.Code.ToLower().Contains(keyword.ToLower()) ||
                              s.Email.ToLower().Contains(keyword.ToLower())

                        select s;

            return await query.ToListAsync();
        }

        public async Task<List<CustomUser>> GetAllCustomUser()
        {
            var query = from st in _context.CustomUsers.AsQueryable()
                       select st;

            return await query.ToListAsync();
            return null;
        }


    }
}

