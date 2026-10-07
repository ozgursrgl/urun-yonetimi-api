using Microsoft.AspNetCore.Mvc;
using UrunYonetimi.Api.Dtos;
using UrunYonetimi.Api.Services;

namespace UrunYonetimi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService) => _tokenService = tokenService;

    /// <summary>Kullanıcı adı ve şifre ile giriş yapar, JWT token döndürür.</summary>
    /// <remarks>Demo kullanıcılar: admin / Admin123!  —  kullanici / Kullanici123!</remarks>
    [HttpPost("giris")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Giris(GirisDto dto)
    {
        var token = _tokenService.GirisYap(dto.KullaniciAdi, dto.Sifre);
        if (token is null)
            return Unauthorized(new { mesaj = "Kullanıcı adı veya şifre hatalı." });

        return Ok(new { token });
    }
}
