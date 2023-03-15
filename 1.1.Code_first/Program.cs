// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;

//code-first
//cmd yazmadan once derlemek gerekır derlemezsen hata verıcektır
//dotnet ef migrations add [Migration Name] ------>dotnet ef migrations add mig_1

//migrations data baseye kaydetmek ıcın dotnet ef database update komutu kullanılır
//migrations geri alma da  dotnet ef database update [migrations name]


//kod uzerınden migrate etmek


ECommerceDbContex context = new();
 await context.Database.MigrateAsync(); //buradada dotnet ef database update aynı işlem yapılmış olur



public class ECommerceDbContex: DbContext //verı tabanı sınıfına karsılık gelebılmesı ıcın turettık Microsoft.EntityFrameworkCore ındırılmıs olmalı

{
    public DbSet<Product> Products { get; set; } //Products adında bır tablo olucak dedık ve turu product turunde olucak dedık
    public DbSet<Customer> customers { get; set; } //DbSet enttıy model bılgılendırmesını saglar bıldırmezsek tablo olusmaz

    //"DbSet<>" Entity Framework içinde veritabanındaki bir tablonun programatik olarak temsil edilmesini sağlayan
    //bir sınıfın adıdır. "<>" içine yazılan tip, o tablonun veri türünü belirtir. Örneğin, "DbSet<Customer>"
    //veritabanındaki "müşteriler" tablosunun temsil edildiğini belirtir ve "Customer" veri modeli sınıfının verilerinin
    //depolanması için kullanılır.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Temel ayarlar ılerıde detaylancak
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=ECommerceDb;User Id=sa ; Password=sıfre;TrustServerCertificate=true");
    }
}


//Entity
public class Product //burada product tablosu modellemıs olduk ve bunu yukarıda orneklıycegımız ıcın enttıy dıyoruz clasa
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quentity { get; set; }
    public float Price  { get; set; }

}
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

}

