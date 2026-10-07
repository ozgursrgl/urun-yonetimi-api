namespace UrunYonetimi.Api.Exceptions;

/// <summary>İstenen kayıt bulunamadığında fırlatılır (HTTP 404'e çevrilir).</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string mesaj) : base(mesaj) { }
}
