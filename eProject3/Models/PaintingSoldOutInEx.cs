using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class PaintingSoldOutInEx : Base
    {
        [ForeignKey(nameof(PaintingInEx.Id))]
        public int? PaintingInExId { get; set; }

        public float PriceAsSoldOut { get; set; }

        public float MoneyToStudent { get; set; }

        public virtual PaintingInEx? PaintingInExs { get; set; }
    }
}
