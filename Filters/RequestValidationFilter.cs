using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nedo.AspNet.Request.Validation.Contracts;

namespace TodoApi.Filters;

public class RequestValidationFilter : IActionFilter
{
    private readonly IRequestValidationEngine _validator;

    public RequestValidationFilter(IRequestValidationEngine validator)
    {
        _validator = validator;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Iterasi setiap argument yang masuk ke controller action
        foreach (var argument in context.ActionArguments.Values)
        {
            // Skip jika argument null
            if (argument == null) 
                continue;

            // Validasi menggunakan IRequestValidationEngine
            // Sesuai interface: Validate(object request, IRequestValidationContext? context = null)
            var result = _validator.Validate(argument);
            
            // Jika validasi gagal
            if (!result.IsValid)
            {
                // Set response BadRequest dengan detail error
                context.Result = new BadRequestObjectResult(new
                {
                    success = false,
                    errors = result.Errors.Select(e => new
                    {
                        field = e.Field,
                        code = e.Code,
                        message = e.Message
                    })
                });
                
                // Stop execution, controller action tidak akan dijalankan
                return;
            }
        }
        
        // Jika semua validasi pass, lanjut ke controller action
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Method ini dipanggil SETELAH controller action selesai
        // Bisa digunakan untuk logging, modify response, dll
        // Untuk kasus ini tidak perlu implementasi
    }
}