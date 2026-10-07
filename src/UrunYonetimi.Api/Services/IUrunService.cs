using UrunYonetimi.Api.Dtos;

namespace UrunYonetimi.Api.Services;

public interface IUrunService
{
    Task<List<UrunListeDto>> GetUrunlerAsync(string? aramaMetni = null);
    Task<UrunListeDto> GetUrunByIdAsync(int id);
    Task<int> AddUrunAsync(UrunDto dto);
    Task UpdateUrunAsync(int id, UrunDto dto);
    Task DeleteUrunAsync(int id);
}
