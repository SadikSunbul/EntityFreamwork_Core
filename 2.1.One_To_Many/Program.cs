// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//
DenemeContext context = new();

//bıre cok ılıskı 
//buradakı durumu bır ozetlıyelım 
//sımdı her bır calısan sadece bır departmanda oldugunu varsayalım 
//her kısının sadece 1 departmanı olur ama her bır departmanın bırden fazla calısanı olabılır bu durumda bıre cok ılıskı olmus olur

#region Default Convention

//Default yontemınde bıre cok ılıskıyı kurarken foreign key kolonuna karsılık gelen bır property tanımlamak mecburıyetınde degılız eger tanımlamazsak efcore bunu kendısı ayarlıycaktır eger tanımlarsak bızım tanımladıgımızı baz alarak kendısı tanımlamayacaktır 

/*
class Calisan //Dependent Entıtıy  --> kendoı basına tablo olu8stramazlar baglılar  --> foreign key burada olucak 
{
    public int Id { get; set; }
    public string Adi { get; set; }
    //ıstersenız DepartmanId yı kendınızde yaza bılırsınız yonetmek ıstıyorsanız yazınız 
    //Buraya DepartmaId olusturmadık bız ama efcore kendısı bunu dusunerek tabloya departman ıd sını kendısı olusturuyor
    public Departman Departman { get; set; } //blurada 1 e .... nı ııskı yatık 

}

class Departman //Principal Entıty ---> kendı basına tablo olustura bılır baglı degıler 
{
    public int Id { get; set; }
    public string DepartmanAdi { get; set; }
    public ICollection<Calisan> Calisanlar { get; set; } //burayı lar eklememızın sebebı bıden fazla calısan oldugu ıcın dedık  //burada ... com ılıskı yaptık

}
*/
#endregion
#region Data Annotations 
// burada Etrubnute kullanıcaz
//Etrubute dedıgımız seyler ---->  [ForeignKey(nameof(Calisan.Id))]---> [Key]   hgıbı seyler bızım ııcın etrubute dır 
//burada forenkey colonunu tanımlıycam ama adı farklı olucak dıyorsanız bunu kullanmalısınız


class Calisan //buarası Dependent entıtydır ---> forenkey burada tanımlanır
{
    //dependent olmasının sebebı departmana baglı olmasıdır 
    public int Id { get; set; }
    [ForeignKey(nameof(Departman))] //burada dedıkkı altakı DId departma forenkeyı ıle baglanıyor 
    public int DId { get; set; }
    public string Adi { get; set; }

    public Departman Departman { get; set; }

}
class Departman
{
    public int Id { get; set; }
    public string DepartmanAdi { get; set; }
    public ICollection<Calisan> Calisanlar { get; set; }
}

#endregion
#region Fluent API


class Calisan // buarsı Dependet
{
    public int Id{ get; set; }
    public int DId { get; set; }
    public string Adi { get; set; }
    public Departman Departman { get; set; }
}
class Departman
{
    public int Id { get; set; }
    public string DepartmanAdi { get; set; }
    public ICollection<Calisan> Calisanlar { get; set; }
}

#endregion



class DenemeContext : DbContext
{
    public DbSet<Calisan> calisanler { get; set; }
    public DbSet<Departman> departmanlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=One_To_One;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calisan>()
            .HasOne(c => c.Departman)
            .WithMany(c => c.Calisanlar)
            .HasForeignKey(c=>c.DId); //forenkey e gerek yok bıre cokta tanımlamadıgımız halde kenıdısı tanımlar 
    }

}