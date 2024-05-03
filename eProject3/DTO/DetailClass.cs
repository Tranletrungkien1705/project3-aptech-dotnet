using eProject3.Models;

namespace eProject3.DTO
{
    public class DetailClass
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string? StudentName { get; set; }
        public DateTime? StartOfDateClass { get; set; }

        public DateTime? EndOfDateClass { get; set; } 

        public string? Code { get; set; }
        
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
