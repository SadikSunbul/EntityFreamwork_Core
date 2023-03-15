// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

ApplicationDbContext context = new();

#region Generated Value Nedir?
//EF Core'da üretilen değerlerle ilgili çeşitli modellerin ayrıntılarını yapılandırmamızı sağlayan bir konfigürasyondur.
#endregion

#region Default Values

//EF Core herhangi bir tablonun herhangi bir kolonuna yazılım tarafından bir değer gönderilmediği taktirde bu kolona hangi değerin(default value) üretilip yazdırılacağını belirleyen yapılanmalardır.

#region HasDefaultValue
//Static veri veriyor.
#endregion

#region HasDefaultValueSql
//SQL cümleciği
#endregion

#endregion

#region Computed Columns

#region HasComputedColumnSql
//Tablo içerisindeki kolonlar üzerinde yapılan aritmatik işlemler neticesinde üretilen kolondur.
#endregion

#endregion

#region Value Generation

#region Primary Keys
//Herhangi bir tablodaki satırları kimlik vari şekilde tanımlayan, tekil(unique) olan sütun veya sütunlardır.
#endregion

#region Identity
//Identity, yalnızca otomatik olarak artan bir sütundur. Bir sütun, PK olmaksızın identity olarak tanımlanabilir. Bir tablo içerisinde identity kolonu sadece tek bir tane olarak tanımlanabilir.
#endregion

//Bu iki özellik genellikle birlikte kullanılmaktadırlar. O yüzden EF Core PK olan bir kolonu otomatik olarak Identity olacak şekilde yapılandırmaktadır. Ancak böyle olması için bir gereklilik yoktur!

#region DatabaseGenerated

#region DatabaseGeneratedOption.None - ValueGeneratedNever
//Bir kolonda değer üretilmeyecekse eğer None ile işaretliyoruz.
//EF Core'un default olarak PK kolonlarda getirdiği Identity özelliğini kaldırmak istiyorsak eğer None'ı kullanabiliriz.
#endregion

#region DatabaseGeneratedOption.Identity - ValueGeneratedOnAdd
//Herhangi bir kolona Identity özelliğini vermemizi sağlayan bir konfigürasyondur.

#region Sayısal Türlerde
//Eğer ki Identity özelliği bir tabloda sayısal olan bir kolonda kullanılacaksa o durumda ilgili tablodaki pk olan kolondan özellikle/iradlei bir şekilde identity özelliğinin kaldırılması gerekmektedir.(None)
#endregion

#region Sayısal Olmayan Türlerde
//Eğer ki Identity özelliği bir tabloda sayısal olmaan bir kolonda kullaınacaksa o durumda ilgili talbodaki pk olan kolondan iradeli bir şekilde identity özelliğinin kaldırılmasına gerek yoktur. 
//burada fullent apı ye gıdıp oradan verı eklenmelıdır 
/*
 modelBuilder.Entity<Person>()
            .Property(p => p.PersonCode)  //ıdentıty dedık buna burası olmadan ınt den farklı bır tur ıcın burasının yazılmas ıyerekır 
            .HasDefaultValueSql("NEWID()"); //guıd ıse turu default degerını verıyoz 
 */
#endregion

#endregion

#region DatabaseGeneratedOption.Computed - ValueGeneratedOnAddOrUpdate
//EF Core üzerinde bir kolon Computed column ise ister Computed olarak belirleyebilirsiniz isterseniz de belirmeden kullanmaya devam edebilirsiniz.
#endregion

#endregion

#endregion

Person p = new()
{
    Name = "Gençay",
    Surname = "Yıldız",

    Premium = 10,
    TotalGain = 110
};
await context.Persons.AddAsync(p);
await context.SaveChangesAsync();

class Person
{
    //[DatabaseGenerated(DatabaseGeneratedOption.None)]  //bunu none ye cekmezsek ha aalıcakgız cunkı ıdentıty sadece 1 tabloda 1 colonu artırır   --> burada ıdentıty ozelıgını kaldırdık  farklı bır turde ıse kalkmaz buradakı ıdentıty guıd
    public int PersonId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Premium { get; set; }
    public int Salary { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]  //gerek yok aslında 
    public int TotalGain { get; set; }
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //artık ıdentıty sendsın dedık burada
    public Guid PersonCode { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .Property(p => p.Salary)
            //.HasDefaultValue(100);
            .HasDefaultValueSql("FLOOR(RAND() * 1000)"); //her seferınde rastgele bır say uretıcektır

        modelBuilder.Entity<Person>()
            .Property(p => p.TotalGain)
            .HasComputedColumnSql("([Salary] + [Premium]) * 10")
            .ValueGeneratedOnAddOrUpdate();  //ValueGeneratedOnAddOrUpdate() gerek yok aslında hıc bır + sı yok bunun computed oldugunu gosterır yazmasakta calısır

        modelBuilder.Entity<Person>()
            .Property(p => p.PersonId)
            .ValueGeneratedNever();  //deger verme dedık burada default ıdentıty degerını kapadık

        modelBuilder.Entity<Person>()
            .Property(p => p.PersonCode)  //ıdentıty dedık buna burası olmadan ınt den farklı bır tur ıcın burasının yazılmas ıyerekır 
            .HasDefaultValueSql("NEWID()"); //guıd ıse turu default degerını verıyoz 

        modelBuilder.Entity<Person>()
            .Property(p => p.PersonCode)
            .ValueGeneratedOnAdd();   //deger ver dedık ıdentıty yaptık
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Shadow;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");
    }
}