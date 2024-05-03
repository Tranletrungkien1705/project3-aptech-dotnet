using eProject3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.DTO
{
    public class DetailSubInClass
    {
        public int Id { get; set; }
        public int StaffName { get; set; }
                   
        public int ClassName { get; set; }
        public int ManagerName { get; set; }
        public int SubjectName { get; set; }

        public int NumberOfSession { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
