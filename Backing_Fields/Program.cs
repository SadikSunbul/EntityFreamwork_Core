// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;




#region Backing Fields
//Tabl ıcerısındekı kolonların entıty klasları ıcerısınde propertıler ıle degıl fıellar ıle temsıl etmemızı saglayan bır ozellıktır

class Person
{
    public int Id { get; set; }
    public string name; //field burası
    //name.Substring(0,3) burada ısımın 0dan 3 kadar olan gerını foster dedık
    public string Name { get =>name.Substring(0,3); set =>name=value; }  //fieldı bagladık burada
    /*
     C# dilinde => sembolü lambda ifadesini temsil eder. Lambda ifadeleri, fonksiyonel programlama konseptlerinden biridir ve fonksiyonları bir değişkene atayarak veya parametre olarak geçerek kullanmayı sağlar.

Lambda ifadeleri, özellikle LINQ (Language Integrated Query) sorguları gibi durumlarda sıkça kullanılır. Bu sorgularda, veri kaynaklarından veri çekmek için kullanılan fonksiyonlar lambda ifadeleri ile yazılabilir.

Örnek olarak, aşağıdaki kodda bir dizi oluşturulmuş ve bu dizideki elemanlar lambda ifadesi ile filtrelenerek yeni bir diziye atanmıştır:
     
    public string Depatment { get; set; }
}
*/
#endregion

#region BackingField Attirbutes
/*
class Person
{
    public int Id { get; set; }
  
    public string name;
    [BackingField(nameof(name))] //burada sadece fıeld kullanılır Name kullanılmaz
    public string Name { get ; set; }
    public string Departman { get; set; }
}
*/
#endregion

#region HasField Fullent API
//fullent ıpa da hashfield metodu backıngfıeld ozellıgıne karsılık gelmektedır
/*
class Person
{
    public int Id { get; set; }

    public string name;
    public string Name { get; set; }
    public string Departman { get; set; }
}
*/
#endregion

#region Fieald And Property Access
//efcorede sorgulama surecınde entıty ıcerısndekı proertylerı yada fıeld ları kullanıp kullanmayacagının davranısını bızlere belırtmektedir.

//ef core hıcbır ayarlama yoksa varsayılan propertyler uzerınden  verılerı ısler egerkı backıng fıeld bıldırılıyorsa fıeld uzerınden ısler eger backıngfıeld bıldırıldıgı halde davranıs belırtılıyorsa ne belırtıyorsa ona gore ıslemeyı devam ettırır

//UsePropertyAccessMode üzerinden davranıs modellemesı gerceklestırıle bılır
/*
class Person
{
    public int Id { get; set; }

    public string name;
    public string Name { get; set; }
    public string Departman { get; set; }
}
*/
#endregion


#region Field-Only Properties
//pek ıhtıyacımız olıycak buna 

//Entıtyler de degerlerıA Almak ıcın propertyler yerıne metotların kullanıldıgı veya belırlı alanalrda hıc gosterılmemesi gerektigi durumlarda (ornegın pk kolonu) kullanılabılır

class Person
{
    public int Id { get; set; }

    public string name;  //bas harfını N yapsanda gelmez
   // public string Name { get; set; }  --> bunu sılıp drekt oalrak ustekın kullancaksak
    public string Departman { get; set; }

    public string GetName()
        => name;
    public string SetName(string value)
        =>this.name = value;
}
#endregion

class BackingFieldDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=VeriSilme;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
        modelBuilder.Entity<Person>()
            .Property(p => p.Name) //name propertysı 
            .HasField(nameof(Person.name))
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        */

        modelBuilder.Entity<Person>()
            .Property(nameof(Person.name)); //bunu yazınca Name propertysıne karsılık gelıcektır 

    }
    //Field : Veri erişim süreçlerinde sadece field'ların kullanılmasını söyler. Eğer field'ın kullanılamayacağı durum söz konusu olursa bir exception fırlatır.
        //FieldDuringConstruction : Veri erişim süreçlerinde ilgili entityden bir nesne oluşturulma sürecinde field'ların kullanılmasını söyler.,
        //Property : Veri erişim sürecinde sadece propertynin kullanılmasını söyler. Eğer property'nin kullanılamayacağı durum söz konusuysa (read-only, write-only) bir exception fırlatır.
        //PreferField,
        //PreferFieldDuringConstruction,
        //PreferProperty
}