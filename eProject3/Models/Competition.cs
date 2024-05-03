namespace eProject3.Models
{
    public class Competition: Base
    {
        public string? Name { get; set; }

        public DateTime? StartOfDate { get; set; }

        public DateTime? EndOfDate { get; set; }

        public string? Description { get; set; }

        public string? Conditions { get; set; }
    }
}
