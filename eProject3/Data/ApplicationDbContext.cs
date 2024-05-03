using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eProject3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<Class> Classes{ get; set; }
        public virtual DbSet<Competition> Competitions{ get; set; }
        public virtual DbSet<Customer> Customeres { get; set; }

        public virtual DbSet<Exhibition> Exhibitions { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<MarkOfPaintInComp> MarkOfPaintInComps { get; set; }
        public virtual DbSet<Painting> Paintings { get; set; }
        public virtual DbSet<PaintingInComp> PaintingInComps { get; set; }
        public virtual DbSet<PaintingInEx> PaintingInExs { get; set; }
        public virtual DbSet<PaintingSoldOutInEx> PaintingSoldOutInExs { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentInSub> StudentInSubs { get; set; }
        public virtual DbSet<SubInClass> SubInClasses { get; set; }
        public virtual DbSet<CustomUser> CustomUsers { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

    }
} 