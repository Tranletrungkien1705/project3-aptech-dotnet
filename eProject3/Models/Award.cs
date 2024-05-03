using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class Award: Base
    {
        public string? Name { get; set; }
        public DateTime? AwardDay { get; set; }
        public string? RemarksOfCompetion { get; set; }
        [ForeignKey(nameof(PaintingInComp.Id))]
        public int? PaintingInCompId { get; set; }

        public virtual PaintingInComp? PaintingInComps { get; set; }
    }
}
