using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrunYonetimi.Api.Dtos;
using UrunYonetimi.Api.Services;

namespace UrunYonetimi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrunlerController : ControllerBase
{
    private readonly IUrunService _urunService;

    public UrunlerController(IUrunService urunService) => _urunService = urunService;

    /// <summary>Tüm ürünleri listeler. İsteğe bağlı olarak ada göre arama yapar.</summary>
    /// <param name="ara">Ürün adında aranacak metin (örn: mouse)</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<UrunListeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? ara)
        => Ok(await _urunService.GetUrunlerAsync(ara));

    /// <summary>Kimliği verilen ürünü getirir.</summary>
    /// <param name="id">Ürün kimliği</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UrunListeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
        => Ok(await _urunService.GetUrunByIdAsync(id));

    /// <summary>Yeni ürün ekler. Giriş yapılmış olmalıdır.</summary>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(UrunDto dto)
    {
        var id = await _urunService.AddUrunAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    /// <summary>Var olan bir ürünü günceller. Giriş yapılmış olmalıdır.</summary>
    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UrunDto dto)
    {
        await _urunService.UpdateUrunAsync(id, dto);
        return NoContent();
    }

    /// <summary>Ürünü siler. Yalnızca Admin rolü yetkilidir.</summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        await _urunService.DeleteUrunAsync(id);
        return NoContent();
    }
}
