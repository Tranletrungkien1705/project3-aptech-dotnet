using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class PaintingInComp : Base
    {
        [ForeignKey(nameof(Painting.Id))]
        public int? PaintingId { get; set; }

        [ForeignKey(nameof(Competition.Id))]
        public int? CompetitionId { get; set; }

        public DateTime DateOfPost { get; set; }

        public virtual Painting? Paintings { get; set; }

        public virtual Competition? Competitions { get; set; }
    }
}
