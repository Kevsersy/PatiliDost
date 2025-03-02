namespace PatiliDost.Models;

public class FeaturedWork 
{
    public string? Title { get; set; } 
    public string? Subtitle { get; set; } 
    public string? Description { get; set; } 
    public List<string>? Benefits { get; set; } 
    public string? Footer { get; set; } 
    public List<string>? ImageUrls { get; set; }
    public int Id { get; set; }
}
