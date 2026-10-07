using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UrunYonetimi.Api.Services;

public interface ITokenService
{
    string? GirisYap(string kullaniciAdi, string sifre);
}

/// <summary>
/// Demo amaçlı basit kimlik doğrulama. Kullanıcılar sabit tanımlıdır;
/// gerçek bir projede veritabanından ve hash'lenmiş şifrelerle kontrol edilir.
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    private static readonly Dictionary<string, (string Sifre, string Rol)> Kullanicilar = new()
    {
        ["admin"] = ("Admin123!", "Admin"),
        ["kullanici"] = ("Kullanici123!", "Kullanici")
    };

    public TokenService(IConfiguration config) => _config = config;

    public string? GirisYap(string kullaniciAdi, string sifre)
    {
        if (!Kullanicilar.TryGetValue(kullaniciAdi, out var k) || k.Sifre != sifre)
            return null;

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, kullaniciAdi),
            new Claim(ClaimTypes.Role, k.Rol)
        };

        var anahtar = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(anahtar, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
