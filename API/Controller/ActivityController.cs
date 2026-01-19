using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.Dtos.Activity;
using TodoApi.Models;

namespace TodoApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController : ControllerBase
{
    private readonly AppDbContext _context;

    public ActivityController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var activity = new Activity
        {
            Name = request.Name,
            Description = request.Description,
            UserId = userId
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        return Ok(activity);
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var data = await _context.Activities
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return Ok(data);
    }
}
