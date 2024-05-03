using eProject3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.DTO
{
    public class DetailStaff
    {
        public int Id { get; set; }
        public int? PaintingId { get; set; }
        public int? TeacherId { get; set; }

        public string? Mark { get; set; }

        public string? Feedback { get; set; }
        public string? FileEntity { get; set; }

    }
}
