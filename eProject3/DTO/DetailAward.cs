namespace eProject3.DTO
{
    public class DetailAward
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? AwardDay { get; set; }
        public string? RemarksOfCompetition { get; set; }
        public int? StudentId { get; set; }
        public int? CompetitionId { get; set; }

    }
}
