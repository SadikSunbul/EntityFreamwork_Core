﻿// See https://aka.ms/new-console-template for more information

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

ETicaretContext context = new();

#region AsNoTracking Metodu
//amacımız ---> malıyetı azaltmak tır

//Context üzerinden gelen tüm datalar Change Tracker mekanizması tarafından takip edilmektedir.

//Change Tracker, takip ettiği nesnelerin sayısıyla doğru orantılı olacak şekilde bir maliyete sahiptir. O yüzden üzerinde
//işlem yapılmayacak verilerin takip edilmesi bizlere lüzumsuz yere bir maliyet ortaya çıkaracaktır.

//AsNoTracking metodu, context üzerinden sorgu neticesinde gelecek olan verilerin Change Tracker tarafından takip edilmesini
//engeller.

//AsNoTracking metodu ile Change Tracker'ın ihtiyaç olmayan verilerdeki maliyetini törpülemiş oluruz.

//AsNoTracking fonksiyonu ile yapılan sorgulamalarda, verileri elde edebilir, bu verileri istenilen noktalarda kullanabilir
//lakin veriler üzerinde herhangi bir değişiklik/update işlemi yapamayız.


var kullanicilar = await context.Kullanicilar.AsNoTracking().ToListAsync(); //uzerınde degısıklık yapılmadıgı ıcın takıp koparılabılrı
foreach (var kullanici in kullanicilar)  //AsNoTracking() IQerable kısmında kullanılır
{
    Console.WriteLine(kullanici.Adi);
    kullanici.Adi = $"yeni-{kullanici.Adi}";
    // context.Kullanicilar.Update(kullanici); //boyle yaparsak burada degısıklıklerimızi kaydolucaktır burda manuel bır sekılde guncelleme yaptıgımız ıcın burayı yazmasaydık degısıklıkler kaydolmıycaktı 
    //remove(kullanıcı);
    //update(kullanıcı);
    //bunları yazsan gene sılıncek veya guncellenecek tır
    //genelde tek sebebp update ıslemı yapmıycaksak chhange tracker ı devre dısı bırakırız

    //bız takıp edilmeyen lerı sılme ve guncelleme de bız remove ve update fonk kullanarak yapabılıyorduk 
}
await context.SaveChangesAsync(); //yukaıdakı degısıklıkler verı tabanımıza kaydolmadı cunk takıp edılmıyor

#endregion

#region AsNoTrackingWithIdentityResolution
//CT(Change Tracker) mekanizması yinelenen verileri tekil instance olarak getirir. Buradan ekstradan bir performans kazancı söz konusudur.

//Bizler yaptığımız sorgularda takip mekanizmasının AsNoTracking metodu ile maliyetini kırmak isterken bazen maliyete sebebiyet verebiliriz.(Özellikle ilişkisel tabloları sorgularken bu duruma dikkat etmemiz gerekyior)

//AsNoTracking ile elde edilen veriler takip edilmeyeceğinden dolayı yinelenen verilerin ayrı instancelarda olmasına sebebiyet veriyoruz. Çünkü CT mekanizması takip ettiği nesneden bellekte varsa eğer aynı nesneden birdaha oluşturma gereği duymaksızın o nesneye ayrı noktalardaki ihtiyacı aynı instance üzerinden gidermektedir.

//Böyle bir durumda hem takip mekanizmasının maliyeitni ortadan kaldırmak hemide yinelenen dataları tek bir instance üzerinde karşılamak için AsNoTrackingWithIdentityResolution fonksiyonunu kullanabiliriz.

//var kitaplar = await context.Kitaplar.Include(k => k.Yazarlar).AsNoTrackingWithIdentityResolution().ToListAsync();

//ilişkisel verilerde bunu kullanamk lazım 

//AsNoTrackingWithIdentityResolution fonksiyonu AsNoTracking fonksiyonuna nazaran görece yavaştır/maliyetlidir lakin CT'a nazaran daha performanslı ve az maliyetlidir.

#endregion

#region AsTracking
//Context üzerinden gelen dataların CT tarafından takip ewdilmesini iradeli bir şekilde ifade etmemizi sağlayan fonksiyondur.
//Peki hoca niye kullanalım ?
//Bir sonraki inceleyeceğimiz UseQueryTrackingBehavior metodunun davranışı gereği uygulama seviyesinde CT'ın default olarak devrede olup olmamasını ayarlıyor olacağız. Eğer ki default olarak pasif hale getirilirse böyle durumda takip mekanizmasının ihtiyaç olduğu sorgularda AsTracking fonksiyonunu kullanabilir ve böylece takip mekanizmasını iradeli bir şekilde devreye sokmuş oluruz.
var kitaplar = await context.Kitaplar.AsTracking().ToListAsync();
#endregion

#region UseQueryTrackingBehavior
//EF Core seviyesinde/uygulama seviyesinde ilgili contextten gelen verilerin üzerinde CT mekanizmasının davranışı temel seviyede belirlememizi sağlayan fonksiyondur. Yani konfigürasyon fonksiyonudur.
#endregion

Console.WriteLine();
public class ETicaretContext : DbContext
{
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<Rol> Roller { get; set; }
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Ekişi;User Id=sa ; Password=sıfre;TrustServerCertificate=true");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //conteten gelen tum datalar artık  takıpedılmıycektır ve  burada degıstırmek ıstedıgımız durumlarda ANoTracking kullnamız gerekır
    }
}
public class Kullanici
{
    public int Id { get; set; }
    public string Adi { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Rol> Roller { get; set; }
}
public class Rol
{
    public int Id { get; set; }
    public string RolAdi { get; set; }
    public ICollection<Kullanici> Kullanicilar { get; set; }
}
public class Kitap
{
    public Kitap() => Console.WriteLine("Kitap nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public int SayfaSayisi { get; set; }

    public ICollection<Yazar> Yazarlar { get; set; }
}
public class Yazar
{
    public Yazar() => Console.WriteLine("Yazar nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string YazarAdi { get; set; }
    public ICollection<Kitap> Kitaplar { get; set; }
}


