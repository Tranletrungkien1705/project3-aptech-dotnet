using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class SubInClass : Base
    {
        [ForeignKey(nameof(Staff.Id))]
        public int? StaffId { get; set; }

        [ForeignKey(nameof(Class.Id))]
        public int? ClassId { get; set; }
        [ForeignKey(nameof(Manager.Id))]
        public int? ManagerId { get; set; }
        [ForeignKey(nameof(Subject.Id))]
        public int? SubjectId { get; set; }

        public int NumberOfSession { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Staff? Staffs { get; set; }

        public virtual Class? Classes { get; set; }

        public virtual Manager? Managers { get; set; }

        public virtual Subject? Subjects { get; set; }
    }
}
