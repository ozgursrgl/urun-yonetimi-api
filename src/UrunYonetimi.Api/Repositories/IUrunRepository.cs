using UrunYonetimi.Api.Models;

namespace UrunYonetimi.Api.Repositories;

public interface IUrunRepository
{
    Task<List<Urun>> GetAllAsync(string? aramaMetni = null);
    Task<Urun?> GetByIdAsync(int id);
    Task<bool> KategoriVarMiAsync(int kategoriId);
    Task AddAsync(Urun urun);
    Task UpdateAsync(Urun urun);
    Task DeleteAsync(Urun urun);
}
