# Ürün Yönetimi API

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
