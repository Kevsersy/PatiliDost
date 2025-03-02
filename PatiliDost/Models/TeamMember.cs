namespace PatiliDost.Models
{
    public class TeamMember : NamedEntity
    {
        public string? Role { get; set; }
        public string? ImagePath { get; set; }
        public int Id { get; set; }
    }
}
