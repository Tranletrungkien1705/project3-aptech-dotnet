using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace eProject3.Repository
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        //viet search o day
        Task<List<Student>> GetStudentByKeyword(string keyword);
        Task<List<Student>> GetStudentToClass(int keyword);
    }
    public class StudentRespository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRespository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
        }

        public async Task<List<Student>> GetStudentByKeyword(string keyword)
        {
            var query = from st in _context.Students.AsQueryable()
                        where st.Name.ToLower().Contains(keyword.ToLower()) ||
                              st.Email.ToLower().Contains(keyword.ToLower()) ||
                              st.Code.ToLower().Contains(keyword.ToLower())
                        select st;

            return await query.ToListAsync();
        }
        public async Task<List<Student>> GetStudentToClass(int keyword)
        {
            var studentsNotInClass = from stu in _context.Students.AsQueryable()
                                     where !_context.StudentInSubs.AsQueryable().Any(sis => sis.StudentId == stu.Id) 
                                           //(from su in _context.SubInClasses.AsQueryable()
                                           // join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
                                           // where cl.Id != keyword
                                           // select su.Id).Contains(stu.Id)
                                     select stu;
            //var studentsNotInStudentInSub = from su in _context.SubInClasses.AsQueryable()
            //                                join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
            //                                join sis in _context.StudentInSubs.AsQueryable() on su.Id equals sis.SubInClassId
            //                                join stu in _context.Students.AsQueryable() on sis.StudentId equals stu.Id
            //                                where !(from sis in _context.StudentInSubs.AsQueryable()
            //                                        join stu in _context.Students.AsQueryable() on sis.StudentId equals stu.Id
            //                                        where sis.SubInClassId == keyword && stu.IsDeleted == false
            //                                        select stu)
            //                                select stu;
            var studentsInSubInClassByClassId = (from stu in _context.Students
                                                 where !(from sis in _context.StudentInSubs
                                                         join su in _context.Students on sis.StudentId equals su.Id
                                                         where sis.SubInClassId == keyword && su.IsDeleted == false && sis.IsDeleted == false
                                                         select su).Contains(stu)
                                                 select stu).Distinct();

            return await studentsInSubInClassByClassId.OrderBy(x=>x.Id).ToListAsync();
        }

        //search

    }
}
