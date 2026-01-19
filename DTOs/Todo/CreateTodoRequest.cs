using Microsoft.AspNetCore.Http;
using Nedo.AspNet.Request.Validation.Attributes.Generic;
using Nedo.AspNet.Request.Validation.Attributes.Image;

namespace TodoApi.Dtos.Todo;

public class CreateTodoRequest
{
    // [Required]
    [MaxLength(1)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [ImageType("png")]
    public IFormFile? AttachmentImage { get; set; }

    public IFormFile? AttachmentFile { get; set; }
}

// structured format validation