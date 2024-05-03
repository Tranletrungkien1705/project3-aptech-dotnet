using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class Customer : Base
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [ForeignKey(nameof(PaintingSoldOutInEx.Id))]
        public int? PaintingSoldOutInExId { get; set; }
        public virtual PaintingSoldOutInEx? GetPaintingSoldOutInExs { get; set; }
    }
}
