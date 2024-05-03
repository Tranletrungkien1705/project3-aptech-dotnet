using eProject3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.DTO
{
    public class DetailPaintingInComp
    {
        public int Id { get; set; }
        public int PaintingName { get; set; }

        public int CompetitionName { get; set; }

        public DateTime DateOfPost { get; set; }
        public string? FileEntity { get; set; }

    }
}
