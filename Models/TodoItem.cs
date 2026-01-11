namespace TodoApi.Models;

using Nedo.AspNet.Request.Validation.Attributes.Numeric;
using Nedo.AspNet.Request.Validation.Attributes.String;

public class TodoItem
{
    [PositiveNumber]
    public int Id { get; set; }
    
    [NoSpecialCharacter]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}