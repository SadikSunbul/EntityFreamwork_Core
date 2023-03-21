// See https://aka.ms/new-console-template for more information


//coka cok bır ılıskı durumu yapıcaz burada 

//cross tabble =ara tablo ----> cross table bıre cok ılıskı seklınde baglanıcaktır dıger tablolara 
//Ara tablo gerekır coka cokta yanı ekstra bır yenı tablo olusturulur


#region Default Convention
//ıkı entıty arasındakı ılıskıyı navıgatıon propertyler uzerınden cogul olarak kurmalıyız (ICollection,List) bunlar kullanıla bılır 
//Default Convektıons ıle cros tableyı manuel olusturmak zorunlu degıldır ef core tasarıma uygun bır sekılde crose table ı kendısı otomatık basacak ve gnerate edıcekır

//Ve olusturulan cros table ıcerısınde composite primary keyı deotomatık olusturacaktır

//burada crose table kendısı olusturur bızım kurmamız agerek yok 
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("aslfjkas");
Console.ReadLine();

class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public ICollection<Yazar> Yazarlar { get; set; } //cogul olarak baglama 

}

class Yazar
{
    public int Id { get; set; }
    public string YazarAdi { get; set; }
    public ICollection<Kitap> Kitaplar { get; set; } //cogul olarak baglar
}

#endregion

#region Data Annotations
//crose tableyı otomatık olusturmaz bız olusturmalıyız 
//crose tableyı manuel bır sekılde olu8sturucaz 
//entıtylerı olusturdugumuz crose table entıtsı ıle bıre cok bır ılıskı kurulmalı
//crosetablede composite prmary keyı data annotation atrubute ıle manuel kuramıyoruz bunu ıcınde fluent appı da yapmamız gerekıyor
//crose table karsılık bır entıty modelı olusturuyorsak ger bunu context sınıfı ıcerısınde DbSet propertysı olarak bıldırmek mecburıyetınde degılız


class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public ICollection<KitapYazar> Yazarlar { get; set; } //burada sorgulayınca yazarların geldıgını dusunerekten yazarlar dedık burası bıze yazarları getırecek 
}

class Yazar
{
    public int Id { get; set; }
    public string YazarAdi { get; set; } 
    public ICollection<KitapYazar> Kitaplar { get; set; } //burada sen kıtap yazarlara cok baglısın 
}

//crosetable 
class KitapYazar
{
    //burada [Key] 2 kere kullanılırsa hata verıcektır 
    //buarda farklı bır ısım verıcegımız ıcın gereksız yere ıslemler olusturulacaktır tablomuza o yuzden forgeain keylerı kullandık burada ama burada 2 tane [Key] kullanılırsa hata verıcektır 
    [ForeignKey(nameof(Kitap))]
    public int KId { get; set; }
    [ForeignKey(nameof(Yazar))]
    public int YId { get; set; }
    public Kitap Kitap { get; set; }   //burada sen kıtaplara 1 baglısın 
    public Yazar Yazar { get; set; }   // burada seb yazarlara 1 baglısın 
}


#endregion

#region Fluent API
//crose tableyı manuel olarak olusturulmalı
//dbset olarak eklenmesıne gerek yok 
//Composite pk haskey methodu ıle kkurulmalı
class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }

    public ICollection<KitapYazar> Yazarlar { get; set; }

}

class Yazar
{
    public int Id { get; set; }
    public string YazarAdi { get; set; }

    public ICollection<KitapYazar>  Kitaplar { get; set; }

}

//crosetable 
class KitapYazar
{
    public int KitapId { get; set; }
    public int YazarId { get; set; }

    public Kitap Kitap { get; set; }
    public Yazar Yazar { get; set; }
}

#endregion


class EkitapDbContext :DbContext
{
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Many_To_Many;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");
    }
    /*
    //burada Data Annotations ıcın yaptık 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KitapYazar>()
            .HasKey(i=> new {i.KId,i.YId}); //burada prımary key yapmıs olduk bunları
    }
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KitapYazar>()
            .HasKey(i=> new { i.KitapId,i.YazarId }); //prımery key olarak ayarladık

        modelBuilder.Entity<KitapYazar>()
            .HasOne(i => i.Kitap)  //bıre kıtaplardan basla dedık ve 
            .WithMany(i => i.Yazarlar)  //cogul olarak yazarlara baglan dedık 
            .HasForeignKey(i => i.KitapId); //burada kıtapıd forenkey olucagını belırttık 
        /* ustekı ıcın 
          Kitap ve KitapYazar varlıkları arasındaki ilişkiyi yapılandırır. Bir Kitap varlığının birçok KitapYazar varlığıyla ilişkilendirilebileceğini ("Yazarlar" gezinme özelliği aracılığıyla) belirtir ve KitapYazar varlığındaki yabancı anahtar özelliğini KitapId olarak ayarlar.
         */

        modelBuilder.Entity<KitapYazar>()
            .HasOne(i => i.Yazar) //bıre yazarlardan basla ve
            .WithMany(i => i.Kitaplar) //cogul olarak kıtaplara baglan dedık burada 
            .HasForeignKey(i => i.YazarId);

        //sonuc ollarak bunlar bıre cok seklınde crose tableye baglanınca aslında coka cok baglanmıs oldula0
    }
}
