# Ürün Yönetimi API

Staj sürecinde öğrendiğim konuları uygulamak için hazırladığım kişisel örnek projedir.
Kurum projesine ait kod içermez.

## İçerik

- ASP.NET Core 8 Web API
- Katmanlı mimari: Controller → Service → Repository
- Entity Framework Core + SQL Server (migration, seed data, `Include` ile eager loading)
- Dependency Injection
- Data Annotations ile validasyon
- JWT ile kimlik doğrulama ve rol bazlı yetkilendirme (Admin / Kullanici)
- Swagger dokümantasyonu (XML summary açıklamalarıyla)
- Ürün adına göre arama
- Merkezi hata yönetimi ve loglama (middleware)
- xUnit birim testleri (Arrange-Act-Assert, sahte repository)

## Proje Yapısı

```
src/UrunYonetimi.Api
  Controllers/    UrunlerController, AuthController
  Services/       UrunService (iş kuralları), TokenService (JWT)
  Repositories/   UrunRepository (veritabanı erişimi)
  Data/           AppDbContext
  Models/         Urun, Kategori
  Dtos/           UrunDto, UrunListeDto, GirisDto
  Middleware/     HataYonetimiMiddleware
tests/UrunYonetimi.Tests
  UrunServiceTests
```

## Çalıştırma

1. `UrunYonetimi.sln` dosyasını Visual Studio 2022 ile açın.
2. `appsettings.json` içindeki bağlantı cümlesini kendi SQL Server'ınıza göre düzenleyin
   (varsayılan: LocalDB).
3. **Tools → NuGet Package Manager → Package Manager Console** açın, Default project olarak
   `UrunYonetimi.Api` seçin ve şunları çalıştırın:
   ```
   add-migration IlkKurulum
   update-database
   ```
4. F5 ile çalıştırın; Swagger sayfası açılır.

## Kullanım

| Metot | Adres | Yetki |
|---|---|---|
| GET | `/api/urunler?ara=mouse` | Herkes |
| GET | `/api/urunler/{id}` | Herkes |
| POST | `/api/auth/giris` | Herkes |
| POST | `/api/urunler` | Giriş yapmış kullanıcı |
| PUT | `/api/urunler/{id}` | Giriş yapmış kullanıcı |
| DELETE | `/api/urunler/{id}` | Sadece Admin |

Demo kullanıcılar: `admin / Admin123!` ve `kullanici / Kullanici123!`

Swagger'da önce `/api/auth/giris` ile token alın, sağ üstteki **Authorize** butonuna yapıştırın.

## Testler

**Test → Run All Tests** veya terminalde `dotnet test`.
