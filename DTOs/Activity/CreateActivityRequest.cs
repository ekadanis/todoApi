using Microsoft.AspNetCore.Http;
using Nedo.AspNet.Request.Validation.Attributes.Generic;

namespace TodoApi.Dtos.Activity;

public class CreateActivityRequest
{
    // [Required]
    [MaxLength(2)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
