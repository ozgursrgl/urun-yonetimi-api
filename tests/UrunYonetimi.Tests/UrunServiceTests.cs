using Microsoft.Extensions.Logging.Abstractions;
using UrunYonetimi.Api.Dtos;
using UrunYonetimi.Api.Exceptions;
using UrunYonetimi.Api.Services;

namespace UrunYonetimi.Tests;

public class UrunServiceTests
{
    private readonly SahteUrunRepository _repo = new();
    private readonly UrunService _service;

    public UrunServiceTests()
    {
        _service = new UrunService(_repo, NullLogger<UrunService>.Instance);
    }

    private static UrunDto GecerliDto(string ad = "Kablosuz Mouse") =>
        new() { UrunAdi = ad, Fiyat = 349.90m, Stok = 10, KategoriId = 1 };

    [Fact]
    public async Task AddUrun_GecerliVeri_YeniIdDoner()
    {
        // Arrange
        var dto = GecerliDto();

        // Act
        var id = await _service.AddUrunAsync(dto);

        // Assert
        Assert.Equal(1, id);
        Assert.Single(_repo.Urunler);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddUrun_AdBos_ArgumentExceptionFirlatir(string ad)
    {
        var dto = GecerliDto(ad);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddUrunAsync(dto));
    }

    [Fact]
    public async Task AddUrun_KategoriYok_ArgumentExceptionFirlatir()
    {
        var dto = GecerliDto();
        dto.KategoriId = 99;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddUrunAsync(dto));
    }

    [Fact]
    public async Task GetUrunById_UrunYok_NotFoundExceptionFirlatir()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetUrunByIdAsync(42));
    }

    [Fact]
    public async Task GetUrunler_AramaMetni_SadeceEslesenleriDoner()
    {
        await _service.AddUrunAsync(GecerliDto("Kablosuz Mouse"));
        await _service.AddUrunAsync(GecerliDto("Mekanik Klavye"));

        var sonuc = await _service.GetUrunlerAsync("mouse");

        Assert.Single(sonuc);
        Assert.Equal("Kablosuz Mouse", sonuc[0].Ad);
    }

    [Fact]
    public async Task DeleteUrun_VarOlanUrun_ListedenSilinir()
    {
        var id = await _service.AddUrunAsync(GecerliDto());

        await _service.DeleteUrunAsync(id);

        Assert.Empty(_repo.Urunler);
    }
}
