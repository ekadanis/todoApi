using Microsoft.AspNetCore.Http;
using Nedo.AspNet.Request.Validation.Attributes.Generic;

namespace TodoApi.Dtos.Todo;

public class CreateTodoRequest
{
    [Required]
    [MaxLength(1)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    // [ImageType("image/png")]
    // [PortraitImage]
    public IFormFile? AttachmentImage { get; set; }

    // 📄 FILE UMUM
    // [FileMaxSize(5 * 1024 * 1024)]
    // [FileMimeType(
    //     "application/pdf",
    //     "application/msword"
    // )]
    public IFormFile? AttachmentFile { get; set; }
}
