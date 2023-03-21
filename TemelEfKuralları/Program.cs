// See https://aka.ms/new-console-template for more information



//code-first
//cmd yazmadan once derlemek gerekır derlemezsen hata verıcektır
//dotnet ef migrations add [Migration Name] ------>dotnet ef migrations add mig_1

//migrations data baseye kaydetmek ıcın dotnet ef database update komutu kullanılır
//migrations geri alma da  dotnet ef database update [migrations name]


//kod uzerınden migrate etmek

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


#region Veri nasıl eklenır
ETicaretContext context = new(); //verı ekleme sılme vb ıslemler bunun uzerınden geerceklesır

await context.Database.MigrateAsync(); // burada tabloyu ılk basta olusturuyoruz sonra ekleme vb ıslemlerı yapabılırız


Ürün urun = new()
{
    //burada Id ataması yaparsak hata alıcaz cunku burada Id yı sql servıs kendısı atanacak sekılde ayarlanmıstır
    //burada atama yapılırsa ahataverır 
    Ürün_adı = "A Ürünü",
    Fiyat = 1000
};

#region context.AddAsync fonk
await context.AddAsync(urun);
#endregion

#region context.DbSet.AddAsync fonk 
await context.ürüns.AddAsync(urun); //yukarıdakı ıle arasındakı fark tıp guvenlı bburası 
#endregion
#region SaveChanges Nedir?  //degısıklerı kaydetmek ıcın kulkanılan bır fonk 

await context.SaveChangesAsync();



//SaveChanges ; insert ,update  ve delete sotgularını oluşturup bir transections eşlığınde verı tabanına gonderip execute eden fonk dur
//eğerki luşturulan sorgulardan  herhangı bırı basarısız olursa tum ıslemler gerı alınır(rollback)
#endregion


#endregion
#region EF Core  Açısından Bir verinin Eklenmesi Gerektiği Nasıl anlaşılır?

ETicaretContext context = new();
Ürün urun = new Ürün()
{
    Ürün_adı = "B ürünü",
    Fiyat = 2000
};

Console.WriteLine(context.Entry(urun).State); //bu komut bıze entity nın nasıl bır bılgısı oldugunu gostrır bıze burda detached  ---> bısey yok bılınmıyor

await context.AddAsync(urun); //burada urunu sql de yerıne koyduk

Console.WriteLine(context.Entry(urun).State); //burada added der yanı eklenecek bısey var dedı 

await context.SaveChangesAsync(); //degısıklıklerı kaydettık

Console.WriteLine(context.Entry(urun).State); //Unchanged=Değişmemiş bı oncesınden veri tabanına eklendıgınden dolayı degısıklık yapılmadı dedı

#endregion
#region Birden fazla Veri eklerken nelere dikkat edilmelidir?

#region SaveChanges i verimli kullanımı
//SaveChanges fonk her tetıklendıgınde bır transectıons olusturacagından ef core ıle yapılan her bır ısleme ozel
//Kullanmaktan kacınmalıyız cunku her ısleme ozel transectıons verıtabanı acısından ekstra bır malıyet demektır
// o uzden mumkun mertebe tum ıslemlerımızı tek bır transectıons eslıgınde verı tabanına gonderebilmek ve yonetebilmek
//için  SaveChanges ı asagıdakı gıbı tek seferde kullanmak hem malıyet hem de yonetıle bılırlık acısından katkıda bulunmus olacaktır


ETicaretContext context = new();
Ürün urun1 = new Ürün()
{
    Ürün_adı = "C ürünü",
    Fiyat = 2000
};
Ürün urun2 = new Ürün()
{
    Ürün_adı = "D ürünü",
    Fiyat = 2000
};
Ürün urun3 = new Ürün()
{
    Ürün_adı = "E ürünü",
    Fiyat = 2000
};

await context.AddAsync(urun1);
//await context.SaveChangesAsync();  //transections malıyettır bızımıcın 3 farklı ıslem ıcın tek bı transections kullanmak gerekır

await context.AddAsync(urun2);
//await context.SaveChangesAsync();

await context.AddAsync(urun3);
await context.SaveChangesAsync(); //tum ıslemlerı yaptıktan sonra bunu yazasak olur herbır ıslem altına yazılırsa bosa malıyet yapıcaktır


#endregion
#region AddRange

ETicaretContext context = new();
Ürün urun1 = new Ürün()
{
    Ürün_adı = "F ürünü",
    Fiyat = 2000
};
Ürün urun2 = new Ürün()
{
    Ürün_adı = "G ürünü",
    Fiyat = 2000
};
Ürün urun3 = new Ürün()
{
    Ürün_adı = "H ürünü",
    Fiyat = 2000
};
await context.ürüns.AddRangeAsync(urun1, urun2, urun3); //ıcerısıne dızı veya , ile gonderıle bılır overıdesı 2 tanedır
await context.SaveChangesAsync();

#endregion
#endregion
#region Eklenen verinin Generate edilen Id'sinin elde etme

ETicaretContext context = new();
Ürün urun1 = new Ürün()
{
    Ürün_adı = "Sadık ürünü",
    Fiyat = 10000
};
await context.AddAsync(urun1);
await context.SaveChangesAsync();
Console.WriteLine(urun1.Id); //KENDI OLUSTURDUGU ID YI ALMAK ISTERSEK BOYLE YAPSAK GELIR BIR USTE DATABASEYE EKLENDI
//BIZE ID DEGERINI DONDURCEKTIR
Console.ReadLine();

#endregion


#region Veriler Nasıl güncellenir?  //guncelleme ıcın ılk once elde etmek gerekır sonra guncellenır

ETicaretContext context = new(); //yaptıgımız ekleme sılme guncelleme vb ıslemlerı ıcın gereklı olan contextir

Ürün gelenurun = await context.ürüns.FirstOrDefaultAsync(u => u.Id == 3);  //ılerıde teferatlı anlatılıcak burası select sorgusu gıbı calısacak
//FirstOrDefaultAsync bu komut ıstedıgımı sarta uygun olanlardan 1. sını getırır bıze
gelenurun.Ürün_adı = "Q ürünü";
gelenurun.Fiyat = 9999;

await context.SaveChangesAsync(); //uste bır guncelele durumu oldugunun farkına varıcak ve guncellıycek

#endregion
#region ChangeTracker Nedir? Kısaca   takıp mekanızması
//ChangeTracker context uzerınden gelenn verılerın takıbınden sorumlu bır mekanızmadır.Bu takıp mekanızması
//sayesınde context uzerınde gelen verılerle ılgılı ıslemler netıcesınde update yahut delete sorgularının
//olusturulacagı anlasılır
#endregion
#region Takip edilmeyen nesneler nasıl güncellenir

ETicaretContext contex = new();
Ürün urun = new()  //burada normal bı sekılde urun olusturduk sql serverdan ıd sını bulup alamdık oyuzden takıp edılmez bu 
{
    Id = 3,
    Fiyat = 888,
    Ürün_adı = "Yeni ürün"
};
#region Update fonksıyonu
//Chacge Tracker mekanızması tarafından takip edilmeyen nesnelerin güncellenebilmesi için update fonk kullanıılır
//update fonk kullana bılmek ıcın kesınle ılgılı nesnede Id degrı verılmelıdır bu deger guncelenecek(update sorgusu olusturulacak )verının hangısı oldugunu ifade edicek

contex.ürüns.Update(urun);
await contex.SaveChangesAsync();

#endregion

#endregion
#region EntitySatate Nedir?
//bır entıty ınstısın durumunu ifade eden bir referanstır

ETicaretContext context = new();
Ürün u = new Ürün();
Console.WriteLine(context.Entry(u).State);

#endregion
#region EF Core açısından bir verinin güncellenmesi Gerektiği nasıl anlaşılır

ETicaretContext context = new();
Ürün urun = await context.ürüns.FirstOrDefaultAsync(i => i.Id == 3);
Console.WriteLine(context.Entry(urun).State); //nchanged

urun.Ürün_adı = "Tahiri";

Console.WriteLine(context.Entry(urun).State); //Modified guncellemeye hazır

await context.SaveChangesAsync();

Console.WriteLine(context.Entry(urun).State); //Unchanged -->Değişmemiş  SaveChangesAsync bu noktadan sonra bır degısıklık yapılmadıgı ıcın bunu dondurur

Console.ReadLine();

#endregion
#region Birden fazla veri güncelenirken Nelere dikat edilmelidir

ETicaretContext context = new();

var ürünler = await context.ürüns.ToListAsync(); //tüm ürünlerı lısteletık 
//contexten gelen verılerın hepsı ChangeTracker den takıbe alınır

foreach (var ürün in ürünler)
{
    ürün.Ürün_adı += "*"; //burada sonlarına * koyduk 
                          // await context.SaveChangesAsync(); //buraya konulursa yanlıs yapmıs oluruz cunku sureklı calısacagı ıcın sıstemı bosa yoracaktır
}
await context.SaveChangesAsync();//burası daha az malıyetlıdır

#endregion 


#region Veri nasıl silinir?

ETicaretContext context = new();
Ürün urun = await context.ürüns.FirstOrDefaultAsync(i => i.Id == 5); //sılme ıslemı yapıcagım verıyı elde ettım
context.Remove(urun);
await context.SaveChangesAsync();

#endregion
#region Silme işlemine ChangedTracker'in görevi
//ChangeTracker context uzerınden gelenn verılerın takıbınden sorumlu bır mekanızmadır.Bu takıp mekanızması
//sayesınde context uzerınde gelen verılerle ılgılı ıslemler netıcesınde delet yahut delete sorgularının
//olusturulacagı anlasılır
#endregion
#region Takip edilemeyen nesneler nasıl silinir

ETicaretContext context = new();
Ürün sil = new()
{
    Id = 2 //dıger ısım fıyat oemlı degıl
};
context.Remove(sil);
await context.SaveChangesAsync();

#region EntityState ile Silme işlemleri  bilmesekte olur burayı hangısıseversen onla yap

ETicaretContext context = new();

Ürün sil = new() { Id = 1 };
context.Entry(sil).State = EntityState.Deleted;
await context.SaveChangesAsync();

#endregion
#endregion
#region Birden fazla veri silinirken Nelere Dikat Edilmelidir?
#region SaveChanged in verimli kullanımı
#endregion
#region Remove Range

ETicaretContext context = new();
List<Ürün> silinecekler = await context.ürüns.Where(i => i.Id >= 7 && i.Id <= 9).ToListAsync(); //burada ıd 7 den buyuk ve esıt 9 dan kucuk ve esıt olanalrı sectık burada
context.ürüns.RemoveRange(silinecekler);
await context.SaveChangesAsync();

#endregion
#endregion
Console.WriteLine();


public class ETicaretContext : DbContext //bızım burada ekle sılme vb yapıcagımız ızlemler ıcın olusturduk
{
    public DbSet<Ürün> ürüns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Proviedr yapılandırması
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=ETicaret;User Id=sa ; Password=sıfre" +
            ";TrustServerCertificate=true");
        //ConnectionsString
        //Lazy Loading
        //vb.

    }

}
public class Ürün
{
    //defoult olarak Id ıcerenlerı kendısı prymerıy key olarak tanımlar kendısı
    public int Id { get; set; }
    public string Ürün_adı { get; set; }
    public int Fiyat { get; set; }
}


#region OnConfiguring ilke konfigrasyon Ayarlarını gerçekleştırmek //yapılandırma var de bnu gırunce
//ef core tool unu yapılandırmak ıcın kullandıgımız bır metotdur
//contex nesnesınde overıde edılerek kullanılmalıdır


#endregion
#region basıt duzeyde entıty tanımlama kuralları
//EFCORE her tablonun defauld olarak primary key kolonu olması gerektıgını kabul eder
//halıyle bu kolonu temsıl eden bır property tanımlamadıgımız takdırde hata vericektir
#endregion
#region Tablo adının belırlenmesi
//
#endregion

