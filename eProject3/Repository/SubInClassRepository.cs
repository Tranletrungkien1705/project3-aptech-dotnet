using eProject3.Data;
using eProject3.Models;
using eProject3.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Policy;

namespace eProject3.Repository
{
    public interface ISubInClassRepository : IBaseRepository<SubInClass>
    {
        //Task<List<DetailSubInClass>> GetDetailSubInClassByAdd(int Id);
        Task<List<SubInClass>> GetDetailSubInClass(int Id);
        SubInClass GetDetailSubInClassById(int id);
        Task<SubInClass> CreateDetailSubInClass(SubInClass entity);
        Task<SubInClass> UpdateDetailSubInClass(SubInClass entity);
        Task<List<SubInClass>> GetSubInClass(int Id);
        Task<List<SubInClass>> GetSubInClassByStudent(int Id);
    }

    public class SubInClassRepository : BaseRepository<SubInClass>, ISubInClassRepository
    {
        private ApplicationDbContext _context;
        private readonly string currentUserName = "";
        public SubInClassRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
            var currentUser = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult();
            if (currentUser != null)
            {
                this.currentUserName = currentUser.UserName;
            }
        }


        //public async Task<List<DetailSubInClass>> GetDetailSubInClassByAdd(int Id)
        //{
        //    var query = from su in _context.SubInClasses.AsQueryable()
        //                join st in _context.Staffs.AsQueryable() on su.StaffId equals st.Id
        //                join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
        //                join ma in _context.Managers.AsQueryable() on su.ManagerId equals ma.Id
        //                join sub in _context.Subjects.AsQueryable() on su.SubjectId equals sub.Id
                        
        //                group su by new
        //                {
        //                    su.Id,
        //                    StaffName = st.Id,
        //                    ClassName = cl.Id,
        //                    ManagerName = ma.Id,
        //                    SubjectName = sub.Id  
                            
        //                } into grouped where grouped.Key.StaffName == Id
        //                select new DetailSubInClass
        //                {
        //                    Id = grouped.Key.Id,
        //                    StaffName = grouped.Key.StaffName,
        //                    ClassName = grouped.Key.ClassName,
        //                    ManagerName = grouped.Key.ManagerName,
        //                    SubjectName = grouped.Key.SubjectName
        //                };
        //    return await query.OrderBy(x => x.Id).ToListAsync();
        //}


        public async Task<List<SubInClass>> GetDetailSubInClass(int Id)
        {
            var query = from su in _context.SubInClasses.AsQueryable()
                        where su.ClassId == Id
                        select su;
            return await query.OrderBy(x => x.Id).ToListAsync();
        }
        public SubInClass GetDetailSubInClassById(int id)
        {
            var x = new SubInClass();
            var query = from su in _context.SubInClasses.AsQueryable()
                        where su.Id == id
                        select su;
            x = query.ToList().FirstOrDefault();
            return x;
        }


        public async Task<SubInClass> CreateDetailSubInClass(SubInClass entity)
        {
            if (entity != null)
            {
                //Console.WriteLine(entity.ToString());
                entity.CreatedUser = currentUserId;
                entity.CreatedTime = DateTime.Now;
                entity.IsDeleted = false;
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public async Task<SubInClass> UpdateDetailSubInClass(SubInClass entity)
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
        public async Task<List<SubInClass>> GetSubInClass(int Id)
        {
            var query = from su in _context.SubInClasses.AsQueryable()
                        join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
                        where su.IsDeleted == false && cl.Id == Id && cl.IsDeleted== false
                        select su;
            return await query.OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<List<SubInClass>> GetSubInClassByStudent(int Id)
        {
            var query = from su in _context.SubInClasses.AsQueryable()
                        join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
                        join sis in _context.StudentInSubs.AsQueryable() on su.Id equals sis.SubInClassId
                        join stu in _context.Students.AsQueryable() on sis.StudentId equals stu.Id
                        where su.IsDeleted == false && cl.Id == Id && cl.IsDeleted == false && stu.Email == currentUserName
                        select su;
            return await query.OrderBy(x => x.Id).ToListAsync();
        }
    }
}