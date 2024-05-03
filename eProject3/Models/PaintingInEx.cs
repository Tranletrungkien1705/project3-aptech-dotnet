using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class PaintingInEx : Base
    {
        [ForeignKey(nameof(Painting.Id))]
        public int? PaintingId { get; set; }

        [ForeignKey(nameof(Exhibition.Id))]
        public int? ExhibitionId { get; set; }

        public float Price { get; set; }

        public bool IsSoldOut { get; set; }

        public virtual Painting? Paintings { get; set; }

        public virtual Exhibition? Exhibitions { get; set; }
    }
}
