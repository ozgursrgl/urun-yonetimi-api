using UrunYonetimi.Api.Dtos;
using UrunYonetimi.Api.Exceptions;
using UrunYonetimi.Api.Models;
using UrunYonetimi.Api.Repositories;

namespace UrunYonetimi.Api.Services;

/// <summary>Ürünlerle ilgili iş kurallarını yönetir.</summary>
public class UrunService : IUrunService
{
    private readonly IUrunRepository _repo;
    private readonly ILogger<UrunService> _logger;

    // Dependency Injection: repository ve logger dışarıdan verilir
    public UrunService(IUrunRepository repo, ILogger<UrunService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<UrunListeDto>> GetUrunlerAsync(string? aramaMetni = null)
    {
        var urunler = await _repo.GetAllAsync(aramaMetni?.Trim());
        return urunler.Select(ListeDtoyaDonustur).ToList();
    }

    /// <exception cref="NotFoundException">Ürün bulunamazsa</exception>
    public async Task<UrunListeDto> GetUrunByIdAsync(int id)
    {
        var urun = await UrunGetirVeyaHataVer(id);
        return ListeDtoyaDonustur(urun);
    }

    /// <summary>Yeni ürün ekler.</summary>
    /// <exception cref="ArgumentException">Ürün adı boşsa veya kategori yoksa</exception>
    /// <returns>Eklenen ürünün Id değeri</returns>
    public async Task<int> AddUrunAsync(UrunDto dto)
    {
        await Dogrula(dto);

        var urun = new Urun
        {
            Ad = dto.UrunAdi.Trim(),
            Aciklama = dto.Aciklama,
            Fiyat = dto.Fiyat,
            Stok = dto.Stok,
            KategoriId = dto.KategoriId,
            EklenmeTarihi = DateTime.Now
        };

        await _repo.AddAsync(urun);
        _logger.LogInformation("Ürün eklendi. Id: {Id}, Ad: {Ad}", urun.Id, urun.Ad);
        return urun.Id;
    }

    public async Task UpdateUrunAsync(int id, UrunDto dto)
    {
        await Dogrula(dto);
        var urun = await UrunGetirVeyaHataVer(id);

        urun.Ad = dto.UrunAdi.Trim();
        urun.Aciklama = dto.Aciklama;
        urun.Fiyat = dto.Fiyat;
        urun.Stok = dto.Stok;
        urun.KategoriId = dto.KategoriId;

        await _repo.UpdateAsync(urun);
        _logger.LogInformation("Ürün güncellendi. Id: {Id}", id);
    }

    public async Task DeleteUrunAsync(int id)
    {
        var urun = await UrunGetirVeyaHataVer(id);
        await _repo.DeleteAsync(urun);
        _logger.LogInformation("Ürün silindi. Id: {Id}", id);
    }

    // ---- Yardımcı metotlar (refactor ile ayrıldı) ----

    private async Task<Urun> UrunGetirVeyaHataVer(int id)
    {
        var urun = await _repo.GetByIdAsync(id);
        if (urun is null)
        {
            _logger.LogWarning("Ürün bulunamadı. Id: {Id}", id);
            throw new NotFoundException($"{id} numaralı ürün bulunamadı.");
        }
        return urun;
    }

    private async Task Dogrula(UrunDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UrunAdi))
            throw new ArgumentException("Ürün adı boş olamaz.");

        if (dto.Fiyat <= 0)
            throw new ArgumentException("Fiyat sıfırdan büyük olmalıdır.");

        if (!await _repo.KategoriVarMiAsync(dto.KategoriId))
            throw new ArgumentException("Seçilen kategori bulunamadı.");
    }

    private static UrunListeDto ListeDtoyaDonustur(Urun u) => new()
    {
        Id = u.Id,
        Ad = u.Ad,
        Aciklama = u.Aciklama,
        Fiyat = u.Fiyat,
        Stok = u.Stok,
        KategoriAdi = u.Kategori?.Ad ?? string.Empty,
        EklenmeTarihi = u.EklenmeTarihi
    };
}
