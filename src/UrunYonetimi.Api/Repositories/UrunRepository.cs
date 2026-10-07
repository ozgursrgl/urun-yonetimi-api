using Microsoft.EntityFrameworkCore;
using UrunYonetimi.Api.Data;
using UrunYonetimi.Api.Models;

namespace UrunYonetimi.Api.Repositories;

public class UrunRepository : IUrunRepository
{
    private readonly AppDbContext _context;

    public UrunRepository(AppDbContext context) => _context = context;

    public async Task<List<Urun>> GetAllAsync(string? aramaMetni = null)
    {
        // Include: kategori bilgisi tek sorguda gelir (N+1 sorunu oluşmaz)
        var sorgu = _context.Urunler
            .Include(u => u.Kategori)
            .AsNoTracking()
            .AsQueryable();

        // Filtreleme veritabanı tarafında yapılır
        if (!string.IsNullOrWhiteSpace(aramaMetni))
            sorgu = sorgu.Where(u => u.Ad.Contains(aramaMetni));

        return await sorgu.OrderBy(u => u.Ad).ToListAsync();
    }

    public Task<Urun?> GetByIdAsync(int id) =>
        _context.Urunler
            .Include(u => u.Kategori)
            .FirstOrDefaultAsync(u => u.Id == id);

    public Task<bool> KategoriVarMiAsync(int kategoriId) =>
        _context.Kategoriler.AnyAsync(k => k.Id == kategoriId);

    public async Task AddAsync(Urun urun)
    {
        _context.Urunler.Add(urun);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Urun urun)
    {
        _context.Urunler.Update(urun);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Urun urun)
    {
        _context.Urunler.Remove(urun);
        await _context.SaveChangesAsync();
    }
}
