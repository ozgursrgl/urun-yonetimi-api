using UrunYonetimi.Api.Exceptions;

namespace UrunYonetimi.Api.Middleware;

/// <summary>Uygulamada oluşan hataları yakalar, loglar ve uygun HTTP koduna çevirir.</summary>
public class HataYonetimiMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HataYonetimiMiddleware> _logger;

    public HataYonetimiMiddleware(RequestDelegate next, ILogger<HataYonetimiMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await Yaz(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ArgumentException ex)
        {
            await Yaz(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen hata. Yol: {Path}", context.Request.Path);
            await Yaz(context, StatusCodes.Status500InternalServerError,
                "Sunucuda beklenmeyen bir hata oluştu.");
        }
    }

    private static Task Yaz(HttpContext context, int kod, string mesaj)
    {
        context.Response.StatusCode = kod;
        return context.Response.WriteAsJsonAsync(new { durum = kod, mesaj });
    }
}
