using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class Painting : Base
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        public string? Conditions { get; set; } // đoạn trích/đoạn thơ/quotes,...

        [ForeignKey(nameof(Student.Id))]
        public int? StudentId { get; set; }

        public virtual Student? Students { get; set; }
        public string? FileEntity { get; set; }
    }
}
