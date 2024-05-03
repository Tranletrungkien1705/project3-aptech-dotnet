using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class Class: Base
    {
        public string? Name { get; set; }

        public DateTime? StartOfDate { get; set; }

        public DateTime? EndOfDate { get; set; }
    }
}
