using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class StudentInSub:Base
    {

        [ForeignKey(nameof(Student.Id))]
        public int? StudentId { get; set; }

        public virtual Student? Students { get; set; }

        [ForeignKey(nameof(SubInClass.Id))]
        public int? SubInClassId { get; set; }

        public virtual SubInClass? SubInClasses { get; set; }
        public string? Descripption { get; set; }
    }
}
