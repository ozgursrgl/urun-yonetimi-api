using UrunYonetimi.Api.Models;
using UrunYonetimi.Api.Repositories;

namespace UrunYonetimi.Tests;

/// <summary>
/// Testlerde gerçek veritabanı yerine kullanılan bellek içi (sahte) repository.
/// Dependency Injection sayesinde servis bunu gerçeğinden ayırt etmez.
/// </summary>
public class SahteUrunRepository : IUrunRepository
{
    private readonly List<Kategori> _kategoriler = new()
    {
        new Kategori { Id = 1, Ad = "Elektronik" }
    };

    public List<Urun> Urunler { get; } = new();

    public Task<List<Urun>> GetAllAsync(string? aramaMetni = null)
    {
        IEnumerable<Urun> sonuc = Urunler;
        if (!string.IsNullOrWhiteSpace(aramaMetni))
            sonuc = sonuc.Where(u => u.Ad.Contains(aramaMetni, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(sonuc.OrderBy(u => u.Ad).ToList());
    }

    public Task<Urun?> GetByIdAsync(int id) =>
        Task.FromResult(Urunler.FirstOrDefault(u => u.Id == id));

    public Task<bool> KategoriVarMiAsync(int kategoriId) =>
        Task.FromResult(_kategoriler.Any(k => k.Id == kategoriId));

    public Task AddAsync(Urun urun)
    {
        urun.Id = Urunler.Count == 0 ? 1 : Urunler.Max(u => u.Id) + 1;
        urun.Kategori = _kategoriler.First(k => k.Id == urun.KategoriId);
        Urunler.Add(urun);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Urun urun) => Task.CompletedTask;

    public Task DeleteAsync(Urun urun)
    {
        Urunler.Remove(urun);
        return Task.CompletedTask;
    }
}
