using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using Nedo.AspNet.Request.Validation.Attributes;

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

    //GET: api/todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable <TodoItem>>> GetTodos()
    {
        return await _context.TodoItems.ToListAsync();
    }

    //Get: api/todo/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if(todo == null)
        {
            return NotFound(new {message = "TodoNotFound"});
        }

        return todo;
    }

    //POST: api/todo
    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem todo)
    {
        if (string.IsNullOrWhiteSpace(todo.Title))
        {
            return BadRequest(new {message = "TItle is required"});
        }

        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id}, todo);
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> UpdateTodo(int id, TodoItem todo)
    {
        if(id != todo.Id)
        {
            return BadRequest(new {message = "ID mismatch"});
        }

        var existingTodo  = await _context.TodoItems.FindAsync(id);
        if(existingTodo == null)
        {
            return NotFound(new {message = "TodoNotFound"});
        }

        existingTodo.Title = todo.Title;
        existingTodo.Description = todo.Description;
        existingTodo.IsCompleted = todo.IsCompleted;
        
        if (todo.IsCompleted && existingTodo.CompletedAt == null)
        {
            existingTodo.CompletedAt = DateTime.UtcNow;        
        }
        else if (!todo.IsCompleted)
        {
            existingTodo.CompletedAt = null;
        }

        try
        {
            await _context.SaveChangesAsync();
        }catch(DbUpdateConcurrencyException)
        {
            if (!TodoExists(id))
            {
                return NotFound();
            }
            throw;
    }
    return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if(todo == null)
        {
            return NotFound(new {message = "Todo Not Found"});
        }

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if(todo == null)
        {
            return NotFound(new {message = "Todo not Found"});
        }

        todo.IsCompleted = !todo.IsCompleted;
        todo.CompletedAt = todo.IsCompleted? DateTime.UtcNow : null;

        await _context.SaveChangesAsync();

        return Ok(todo);
    }

    private bool TodoExists(int id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
}



