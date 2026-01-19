using System.Globalization;
using Nedo.AspNet.Request.Validation.Abstractions;

namespace API.Middlewares;

public sealed class RequestCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IRequestValidationContext validationContext)
    {
        CultureInfo? culture = null;
        var acceptLanguage = context.Request.Headers["Accept-Language"]
            .FirstOrDefault()?.Split(',').FirstOrDefault()?.Split('-')?.FirstOrDefault()?.Trim();

        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            culture = TryCreateCulture(acceptLanguage);
            Console.WriteLine($"🌻🌻DEBUG: cultureHeader = {culture}");  
        }

        if (culture is null && context.User.Identity?.IsAuthenticated == true)
        {
            var jwtLanguage = context.User.FindFirst("language")?.Value;
            culture = TryCreateCulture(jwtLanguage);
            Console.WriteLine($"🌻🌻DEBUG: cultureJWT = {culture}");
        }

        if(culture is not null)
        {
            validationContext.CurrentCulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture; 
        }

        Console.WriteLine($"🌻🌻DEBUG: culture = {culture}");  
        await _next(context);
    }

    private static CultureInfo? TryCreateCulture(string? cultureName)
    {
        if(string.IsNullOrWhiteSpace(cultureName))
            return null;

        try
        {
            return new CultureInfo(cultureName);
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
    }
}


