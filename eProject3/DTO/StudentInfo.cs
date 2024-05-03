using eProject3.Models;

namespace eProject3.DTO
{
    public class StudentInfo
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? SubName { get; set; }
        public string? ClassName { get; set; }
        public string? TeacherName { get; set; }
        public string? Descripption { get; set; }
    }
}
