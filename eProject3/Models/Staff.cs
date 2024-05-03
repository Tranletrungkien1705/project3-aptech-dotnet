using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProject3.Models
{
    public class Staff: Base
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? Degree { get; set; }

    }
}
