using eProject3.Data;
using eProject3.Models;
using eProject3.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eProject3.Repository
{
    public interface IClassRepository : IBaseRepository<Class>
    {
        List<Class> GetClassByStudentId();
        //Task<DetailPaintingInEx> UpdateDetailPaintingInEx(DetailPaintingInEx entity);
        //Task<List<DetailClass>> GetDetailClass();
        //Task<DetailClass> CreateDetailClass(DetailClass entity);
    }

    public class ClassRepository : BaseRepository<Class>, IClassRepository
    {
        private ApplicationDbContext _context;
        private readonly string currentUserName = "";
        public ClassRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
            var currentUser = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult();
            if (currentUser != null)
            {
                this.currentUserName = currentUser.UserName;
            }
        }



        public List<Class> GetClassByStudentId()
        {
            var query = from c in _context.Classes.AsQueryable()
                        join cl in _context.SubInClasses.AsQueryable() on c.Id equals cl.ClassId
                        join st in _context.StudentInSubs.AsQueryable() on cl.Id equals st.SubInClassId
                        join s in _context.Students.AsQueryable() on st.StudentId equals s.Id
                        where s.Email == currentUserName
                        select c;
            return query.OrderBy(x => x.Id).ToList();
        }

        //public DetailClass GetDetailClassById(int id)
        //{
        //    var x = new DetailClass();
        //    var query = from c in _context.Classes.AsQueryable()
        //                join cl in _context.SubInClasses.AsQueryable() on c.Id equals cl.ClassId
        //                join st in _context.StudentInSubs.AsQueryable() on cl.Id equals st.SubInClassId
        //                join s in _context.Students.AsQueryable() on st.StudentId equals s.Id
        //                group c by new
        //                {
        //                    c.Id,
        //                    ClassName = c.Name,
        //                    StartOfDateClass = c.StartOfDate,
        //                    EndOfDateClass = c.EndOfDate,
        //                    StudentName = s.Name
        //                }
        //                into grouped
        //                where grouped.Key.Id == id
        //                select new DetailClass
        //                {
        //                    Id = grouped.Key.Id,
        //                    ClassName = grouped.Key.ClassName,
        //                    StartOfDateClass = grouped.Key.StartOfDateClass,
        //                    EndOfDateClass = grouped.Key.EndOfDateClass,
        //                    StudentName = grouped.Key.StudentName
        //                };
        //    x = query.ToList().FirstOrDefault();
        //    return x;
        //}


        //public async Task<DetailClass> CreateDetailClass(DetailClass entity)
        //{
        //    if (entity != null)
        //    {
        //        //Console.WriteLine(entity.ToString());
        //        var x = new DetailClass();
        //        x.ClassName = entity.ClassName;
        //        x.StartOfDateClass = entity.StartOfDateClass;
        //        x.EndOfDateClass = entity.EndOfDateClass;
        //        x.StudentName = entity.StudentName;
        //        await _dbSet.AddAsync(x);
        //        await _context.SaveChangesAsync();
        //        Console.WriteLine(x);
        //        return entity;
        //    }
        //    return null;
        //}

        //public async Task<DetailPaintingInEx> UpdateDetailPaintingInEx(DetailPaintingInEx entity)
        //{
        //    if (entity != null)
        //    {
        //        var x = new PaintingInEx();
        //        x.Id = entity.Id;
        //        x.PaintingId = entity.PaintingName;
        //        x.ExhibitionId = entity.ExhibitionName;
        //        x.Price = entity.Price;
        //        x.IsSoldOut = entity.IsSoldOut == "ban" ? true : false;
        //        _dbSet.Update(x);
        //        await _context.SaveChangesAsync();
        //        return entity;
        //    }
        //    return null;
        //}



    }
}