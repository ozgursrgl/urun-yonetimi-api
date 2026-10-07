using System.ComponentModel.DataAnnotations;

namespace UrunYonetimi.Api.Dtos;

/// <summary>Ürün ekleme ve güncelleme için kullanılan model.</summary>
public class UrunDto
{
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir")]
    public string UrunAdi { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Aciklama { get; set; }

    [Range(0.01, 1_000_000, ErrorMessage = "Fiyat 0,01 ile 1.000.000 arasında olmalıdır")]
    public decimal Fiyat { get; set; }

    [Range(0, 100_000)]
    public int Stok { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçiniz")]
    public int KategoriId { get; set; }
}

/// <summary>API'nin dışarıya döndürdüğü ürün bilgisi.</summary>
public class UrunListeDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public decimal Fiyat { get; set; }
    public int Stok { get; set; }
    public string KategoriAdi { get; set; } = string.Empty;
    public DateTime EklenmeTarihi { get; set; }
}

public class GirisDto
{
    [Required]
    public string KullaniciAdi { get; set; } = string.Empty;

    [Required]
    public string Sifre { get; set; } = string.Empty;
}
