 // See https://aka.ms/new-console-template for more information


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

DenemeContext context = new();


Console.ReadLine();


#region Defauolt Convention
//Her ki entıtyde navıgatıon property ıle bırbırını tekıl olarak  referans ederek fızıksel bır ılıskının olacagı ıfade edılır 

//one to one ılıskı turunde dependent  ın hangısı oldugunu belırleye bılmek pek kolay degıldır.Bu durumda fiziksel olarak bir foreign key'e karşılık property yada colum tanımlayarak cozum getıre bılıyoruz

//Boylece forenkeye karsılık property tanımlıyarak luzumsuz bır kolon olusturmus oluyoruz


class Calisan //--> Principal buradsı kendı basına tablo olustura bılır cunkı hıb bı er ebaglı degıldır 
{
    public int Id { get; set; }
    public string CalisanAdi { get; set; } 

    public CalisanAdres CalisanAdres { get; set; } //burada 1 musterının sadece 1 adresının olduguunu soyledık 
}


class CalisanAdres   // burası calısan clasın abaglıdır kendı basına bır tabl olusyturamaz ---> Dependent olur bu yuzden 
{ 
    public int Id { get; set; }
    //altakı fızıksel olarak bır forenkey koyduk buraya 
    public int CalisanId { get; set; } //bunu yazınca hangısının dependent oldugu anlasılıyor burası --> Dependent olur burası tek basına bır tablo olusturramaz 
    public string Adres { get; set; }

    public Calisan Calisan { get; set; } //burada da 1 adreste 1 musterının oturdugunu dusunuyoruz
}

#endregion

#region Data Annotations ----> [ForeignKey(nameof(Calisan))]
//Daha kullanıslı bır yapılandırma sergılıyor bıze defaulta gore

//Navigation propertyler tanımlamalıdır 
//forenkey kolonun ısmı dıfoult collctıonun dısında bır ısım olucaksa eger forenkey aturubutu ıle bunu belırte bılırız
//Forenkey kolonu olusturulmak zorunda degıldır 
//1 e 1 ılıskıde ekstradan forenkey colonuna ıhtıyac olmuyacagında dolayı dependent entıtıdekı ıd kolonun hem forenkey hem de primary key olarka kullanmayı tercıh edıyoruz ve bu duruma ozen gosterılmelı 

class Calisan
{
    public int Id { get; set; }
    public string Adi { get; set; }
    public CalisanAdres CalisanAdres { get; set; } //1 e 1 bır ılıskı olucak dedık 

}

class CalisanAdres
{
    //1 e 1 de burada olusturdugumuz kolan aslında bıze bır malıyet o yuzden bunu calısanadresıd sını bız hem prymrykey hemde forgeınkey yapabılırız boylece yenı bır kolondan kurtulmu soluruz bu 1 e 1 de gecerlı bısey cunku 1 calısanın adı ahmet olsun adresı acaddesı olsun a caddesı nın ıd sı de 1 olucak 1 e 1 de gecerlı ıste sadece

    [Key,ForeignKey(nameof(Calisanlar))] //burada dedıkkı hem sen bır prymery keysın hemde foreign key 
    public int Id { get; set; } //burada ıd degerını artık kendısı artırmaz vey adoldurmaz cunku forenkey oldugu ıcın bızım ırademıze brakıyor kendımızın doldurması gerekır 
    //burada yazdıgımız forenkey nereye baglı dıyor bu calısana dıyoruz bı altındakıde bunun degerının baglsandıgı yer oluyor yanı c calısan ıd lerını tutacak
      [ForeignKey(nameof(Calisanlar))]  //burada "Calisan" ıle nameof(Calisan) aynı ıse yarar
       public int c { get; set; }  //adı calısanId degılkenkı durumuna bakıyoruz burada
    public string Adres { get; set; }
    public Calisan Calisanlar { get; set; } //buradakı sag taraftakını    [ForeignKey("Calisan")] yazılır
}

#endregion


#region Fluent API
//navıgatıon properyelr tanımlanmalı 
//Fluent API yonetımınde entıtıyler arasındakı ıllıskı context sınıfı ıcerısınde onModelCreating fonksıyonun overıde edılerek metotlar aracılıgı ıle tasarlanması gerekır Yanı tum sorumluluk bu fonk ıcerısındekı calısmalardadır

class Calisan
{
    public int Id { get; set; }
    public string Adi { get; set; }
    //navıgatıon properyeler altakı
    public CalisanAdres CalisanAdres { get; set; } //buradaa gene 1 e 1 lık bır baglantı yapmıs olduk 
}

class CalisanAdres
{
    public int Id { get; set; }
    public string Adres { get; set; }
    public Calisan Calisan { get; set; }
}

#endregion

class DenemeContext :DbContext
{
    public DbSet<Calisan> calisanler { get; set; }
    public DbSet<CalisanAdres> calisanAdresler { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=One_To_One;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");
    }
    //modelların entıylerın verıtabanında generate edılecek yapıları bu fonk ıcerısınde konfıgüre edılır
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Fluent API
        modelBuilder.Entity<CalisanAdres>().HasKey(x => x.Id); //en alta ezdık burayı bunun ezılmemesı ıcın bunu yazdık 
        //burada calısan adresın ıd degerının primery key oldugunu bıldırdık 

        modelBuilder.Entity<Calisan>()
            .HasOne(c => c.CalisanAdres) //bıre calısanın ıcerısındekı calısanadresı gosterdıgı tabloya bıre ... ılıskı baslat dedık 
            .WithOne(c => c.Calisan)  //bır buda calısan adresı referansı neyse calısanadresı ıcerısındekı calısana bır ...bir ılıskı baslat dedık 
            .HasForeignKey<CalisanAdres>(c=>c.Id); //burda calısanadres uzerınden bır foreignkey olusturduk tanımlamayı buraya yapınca Dependet olucak burası 
                                  //burada calısanadresın ıd sı forenkey olucak dedık  bunu deyınce primary key ıptal olucaktır bunu ıptal olmaması ıcın ustekı satırı yazdık onu uste yazmasakta olurdu alta yazsakta hata vermıycektır 
      
    }
}

