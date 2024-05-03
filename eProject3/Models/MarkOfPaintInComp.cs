using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class MarkOfPaintInComp: Base
    {
        [ForeignKey(nameof(PaintingInComp.Id))]
        public int? PaintingInCompId { get; set; }


        [ForeignKey(nameof(Staff.Id))]
        public int? TeacherId { get; set; }

        public string? Mark { get; set; }

        public string? PositivePoint { get; set; }

        public string? NegativePoint { get; set; }

        public string? Feedback { get; set; }

        public virtual PaintingInComp? PaintingInComps { get; set; }

        public virtual Staff? Teacher { get; set; }
    }
}
