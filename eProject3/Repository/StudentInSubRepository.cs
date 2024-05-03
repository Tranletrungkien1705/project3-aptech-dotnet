using eProject3.Data;
using eProject3.Models;
using eProject3.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Policy;

namespace eProject3.Repository
{
    public interface IStudentInSubRepository : IBaseRepository<StudentInSub>
    {
        //Task<List<DetailSubInClass>> GetDetailSubInClassByAdd(int Id);
        Task<List<StudentInSub>> GetDetailStudentInSub(int Id, int keyword);
        DetailSubInClass GetDetailSubInClassById(int id);
        Subject GetSubInClassById(int id);
        Task<StudentInSub> CreateStudentInSub(StudentInSub entity);
        Task<StudentInfo> UpdateStudentInSub(int id, int keyword);
    }

    public class StudentInSubRepository : BaseRepository<StudentInSub>, IStudentInSubRepository
    {
        private ApplicationDbContext _context;
        public StudentInSubRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
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


        public async Task<List<StudentInSub>> GetDetailStudentInSub(int Id,int keyword)
        {
            var query = from su in _context.SubInClasses.AsQueryable()
                        join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
                        join sis in _context.StudentInSubs.AsQueryable() on su.Id equals sis.SubInClassId
                        where (sis.IsDeleted == false && cl.Id == Id && su.Id == keyword) ||
                      (sis.IsDeleted == null && cl.Id == Id && su.Id == keyword)
            select sis;
            return await query.OrderBy(x => x.Id).ToListAsync();
            //return null;
        }
        public DetailSubInClass GetDetailSubInClassById(int id)
        {
            var x = new DetailSubInClass();
            var query = from su in _context.SubInClasses.AsQueryable()
                        join st in _context.Staffs.AsQueryable() on su.StaffId equals st.Id
                        join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
                        join ma in _context.Managers.AsQueryable() on su.ManagerId equals ma.Id
                        join sub in _context.Subjects.AsQueryable() on su.SubjectId equals sub.Id
                        group su by new
                        {
                            su.Id,
                            StaffName = st.Id,
                            ClassName = cl.Id,
                            ManagerName = ma.Id,
                            SubjectName = sub.Id

                        } into grouped
                        where grouped.Key.Id == id
                        select new DetailSubInClass
                        {
                            Id = grouped.Key.Id,
                            StaffName = grouped.Key.StaffName,
                            ClassName = grouped.Key.ClassName,
                            ManagerName = grouped.Key.ManagerName,
                            SubjectName = grouped.Key.SubjectName
                        };
            x = query.ToList().FirstOrDefault();
            return x;
        }
        public Subject GetSubInClassById(int id)
        {
            var x = new Subject();
            var query = from su in _context.SubInClasses.AsQueryable()
                        join st in _context.Staffs.AsQueryable() on su.StaffId equals st.Id
                        join cl in _context.Classes.AsQueryable() on su.ClassId equals cl.Id
                        join ma in _context.Managers.AsQueryable() on su.ManagerId equals ma.Id
                        join sub in _context.Subjects.AsQueryable() on su.SubjectId equals sub.Id
                        where su.Id == id
                        select sub;
            x = query.ToList().FirstOrDefault();
            return x;
        }


        public async Task<StudentInSub> CreateStudentInSub(StudentInSub entity)
        {
            if(entity != null)
            {
                var StudentInsub = new StudentInSub();
                StudentInsub.SubInClassId = entity.SubInClassId;
                StudentInsub.StudentId = entity.StudentId;
                StudentInsub.CreatedUser = currentUserId;
                StudentInsub.CreatedTime = DateTime.Now;
                StudentInsub.IsDeleted = false;
                await _dbSet.AddAsync(StudentInsub);
                await _context.SaveChangesAsync();
                return StudentInsub;
            }
            return null;
        }

        public async Task<StudentInfo> UpdateStudentInSub(int id, int keyword)
        {
            //if (entity != null)
            //{
            //    var x = new SubInClass();
            //    x.StaffId = entity.StaffName;
            //    x.ClassId = entity.ClassName;
            //    x.ManagerId = entity.ManagerName;
            //    x.SubjectId = entity.SubjectName;
            //    _dbSet.Update(x);
            //    await _context.SaveChangesAsync();
            //    return entity;

            //}
            return null;
        }
    }
}