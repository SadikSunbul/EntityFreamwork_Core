using System;
using System.Collections.Generic;

namespace _1._0.ders1;

public partial class Personeller
{
    public int Id { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? EmailAdresi { get; set; }

    public DateTime? Olusturmatarihi { get; set; }

    public DateTime? Değistirmetarihi { get; set; }

    public bool? Silindimi { get; set; }

    public string? Cinsiyet { get; set; }
}
