namespace UrunYonetimi.Api.Models;

public class Urun
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public decimal Fiyat { get; set; }
    public int Stok { get; set; }
    public DateTime EklenmeTarihi { get; set; } = DateTime.Now;

    public int KategoriId { get; set; }
    public Kategori? Kategori { get; set; }
}
