// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

ESirketDbContext context = new();

Console.WriteLine("");

class ESirketDbContext : DbContext
{
    public DbSet<Calisan> Calisanalar { get; set; }
    public DbSet<CalisanAdresi> CalisanAdresleri { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=One_To_One;User Id=SA ; Password=Viabelli34*.;TrustServerCertificate=true");
    }
}


// 1 e 1 
#region Default Convention 

//burada hangısı hangısıne baglı oldugunu anlıyamayız

class Calisan
{
    public int Id { get; set; }
    public string Adi { get; set; }

    public CalisanAdresi CalisanAdresi { get; set; } //burada 1 calısanın 1 adresının oldugunu anlıyoruz 


}
class CalisanAdresi
{
    public int Id { get; set; }
    public int CalisanId { get; set; }//bunu ekleyınce adreslere calısanı eklıycez dedı burada 
    public string Adres { get; set; }

    public Calisan Calisan { get; set; } //buradada her adreste bır kısı var demı olduk 
}

#endregion

#region Data Annotations

#endregion

#region Fluent API

#endregion





