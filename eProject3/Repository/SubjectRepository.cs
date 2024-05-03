using eProject3.Data;
using eProject3.Models;
using eProject3.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace eProject3.Repository
{
    public interface ISubjectRepository : IBaseRepository<Subject>
    {

    }

    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        private ApplicationDbContext _context;
        public SubjectRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
        }
    }
}
