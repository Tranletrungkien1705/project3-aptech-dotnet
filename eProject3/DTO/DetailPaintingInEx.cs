using eProject3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.DTO
{
    public class DetailPaintingInEx
    {
        public int Id { get; set; }
        public int PaintingName { get; set; }
        public int ExhibitionName { get; set; }

        public float Price { get; set; }

        public string IsSoldOut { get; set; }
        public string? FileEntity { get; set; }
    }
}
