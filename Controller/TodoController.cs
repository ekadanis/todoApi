using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos.Todo;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        return await _context.TodoItems.ToListAsync();
    }

    // GET: api/todo/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo is null)
            return NotFound(new { message = "Todo not found" });

        return todo;
    }

    // POST: api/todo
    [HttpPost]
    public async Task<IActionResult> CreateTodo([FromForm] CreateTodoRequest request)
    {
        var todo = new TodoItem
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // === SAVE IMAGE ===
        if (request.AttachmentImage is not null)
        {
            var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.AttachmentImage.FileName)}";
            var imagePath = Path.Combine("wwwroot", "uploads", "images", imageFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

            await using var stream = System.IO.File.Create(imagePath);
            await request.AttachmentImage.CopyToAsync(stream);

            todo.AttachmentImagePath = $"/uploads/images/{imageFileName}";
        }

        // === SAVE FILE ===
        if (request.AttachmentFile is not null)
        {
            var fileFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.AttachmentFile.FileName)}";
            var filePath = Path.Combine("wwwroot", "uploads", "files", fileFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            await using var stream = System.IO.File.Create(filePath);
            await request.AttachmentFile.CopyToAsync(stream);

            todo.AttachmentFilePath = $"/uploads/files/{fileFileName}";
        }

        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }


    // PUT: api/todo/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, [FromForm] CreateTodoRequest request)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo is null)
            return NotFound(new { message = "Todo not found" });

        // Update data
        todo.Title = request.Title;
        todo.Description = request.Description;
        todo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/todo/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo is null)
            return NotFound(new { message = "Todo not found" });

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PATCH: api/todo/5/toggle
    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo is null)
            return NotFound(new { message = "Todo not found" });

        todo.IsCompleted = !todo.IsCompleted;
        todo.CompletedAt = todo.IsCompleted ? DateTime.UtcNow : null;
        todo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(todo);
    }
}
