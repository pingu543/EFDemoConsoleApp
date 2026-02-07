namespace EFDemoConsoleApp.Models;

// For testing how migrations work when adding new entities and relationships, this Post entity can be commented out and added back later.
public class Post
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }  // Foreign key
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}