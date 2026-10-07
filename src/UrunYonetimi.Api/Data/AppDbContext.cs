using Microsoft.EntityFrameworkCore;
using UrunYonetimi.Api.Models;

namespace UrunYonetimi.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Urun> Urunler => Set<Urun>();
    public DbSet<Kategori> Kategoriler => Set<Kategori>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Urun>(e =>
        {
            e.Property(u => u.Ad).HasMaxLength(100).IsRequired();
            e.Property(u => u.Aciklama).HasMaxLength(500);
            e.Property(u => u.Fiyat).HasPrecision(18, 2);

            // Foreign key: her ürün bir kategoriye aittir
            e.HasOne(u => u.Kategori)
             .WithMany(k => k.Urunler)
             .HasForeignKey(u => u.KategoriId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Kategori>()
            .Property(k => k.Ad).HasMaxLength(50).IsRequired();

        // Başlangıç verileri
        modelBuilder.Entity<Kategori>().HasData(
            new Kategori { Id = 1, Ad = "Elektronik" },
            new Kategori { Id = 2, Ad = "Kırtasiye" },
            new Kategori { Id = 3, Ad = "Ev & Yaşam" });

        var tarih = new DateTime(2026, 1, 1);
        modelBuilder.Entity<Urun>().HasData(
            new Urun { Id = 1, Ad = "Kablosuz Mouse", Fiyat = 349.90m, Stok = 50, KategoriId = 1, EklenmeTarihi = tarih },
            new Urun { Id = 2, Ad = "Mekanik Klavye", Fiyat = 1899.00m, Stok = 20, KategoriId = 1, EklenmeTarihi = tarih },
            new Urun { Id = 3, Ad = "Not Defteri A4", Fiyat = 45.50m, Stok = 300, KategoriId = 2, EklenmeTarihi = tarih },
            new Urun { Id = 4, Ad = "Masa Lambası", Fiyat = 599.00m, Stok = 35, KategoriId = 3, EklenmeTarihi = tarih });
    }
}
