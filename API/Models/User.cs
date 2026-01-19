using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(100)]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // navigation
    public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
