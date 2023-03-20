

using Microsoft.EntityFrameworkCore;
using System.Reflection;

ApplicationDbContext context = new();

#region Complext Query Operators

#region Join

#region Query Syntax
var query = from photo in context.Photos
            join person in context.Persons
                on photo.PersonId equals person.PersonId
            select new
            {
                person.Name,
                photo.Url
            };
var datas = await query.ToListAsync();
#endregion
#region Method Syntax
var query = context.Photos
    .Join(context.Persons,  //fotoyu hangı tablo ıle bırlestıreceksın onu yazıyoruz buraya
    photo => photo.PersonId,  //ilk tablodakı hangı kolon da bırlestırme olucak 
    person => person.PersonId,  //2. tablodakı baglancak colon
    (photo, person) => new
    {
        person.Name,
        photo.Url
    });
/*
 "(photo, person) =>" ifadesi bir lambda ifadesini ifade eder. Lambda ifadeleri, fonksiyonlara benzer şekilde bir girdi alır ve bir çıktı üretirler. Bu durumda, lambda ifadesi "photo" ve "person" adında iki girdi alır ve yeni bir nesne döndürür.
 */

var datas = await query.ToListAsync();
#endregion

#region Multiple Columns Join

#region Query Syntax
var query = from photo in context.Photos
            join person in context.Persons
                on new { photo.PersonId, photo.Url } equals new { person.PersonId, Url = person.Name }
            select new
            {
                person.Name,
                photo.Url
            };
var datas = await query.ToListAsync();
#endregion
#region Method Syntax
var query1 = context.Photos
    .Join(context.Persons,
    photo => new
    {
        photo.PersonId,
        photo.Url
    },
    person => new
    {
        person.PersonId,
        Url = person.Name  //bunun normlade boyle olmaması lazımdı
    },
    (photo, person) => new
    {
        person.Name,
        photo.Url
    });

var datas1 = await query1.ToListAsync();
#endregion
#endregion

#region 2'den Fazla Tabloyla Join

#region Query Syntax
var query = from photo in context.Photos
            join person in context.Persons
                on photo.PersonId equals person.PersonId
            join order in context.Orders
                on person.PersonId equals order.PersonId
            select new
            {
                person.Name,
                photo.Url,
                order.Description
            };

var datas = await query.ToListAsync();
#endregion
#region Method Syntax
var query2 = context.Photos
    .Join(context.Persons,
    photo => photo.PersonId,
    person => person.PersonId,
    (photo, person) => new
    {
        person.PersonId, //alta kullanmak ıcın cunku onun ıle baska bır tabloyubaglıycaz
        person.Name,
        photo.Url
    })
    .Join(context.Orders,
    personPhotos => personPhotos.PersonId,  //ustekı tablo yenı kendımızın olusturdugu
    order => order.PersonId,
    (personPhotos, order) => new
    {
        personPhotos.Name,
        personPhotos.Url,
        order.Description
    });

var datas2 = await query2.ToListAsync();
#endregion
#endregion

#region Group Join - GroupBy Değil!
var query = from person in context.Persons
            join order in context.Orders
                on person.PersonId equals order.PersonId into personOrders
            //from order in personOrders
            select new
            {
                person.Name,
                Count = personOrders.Count(),
                personOrders   //ordera ulasamazsın cunku onu gurupladık
            };
var datas = await query.ToListAsync();
#endregion
#endregion

//DefaultIfEmpty : Sorgulama sürecinde ilişkisel olarak karşılığı olmayan verilere default değerini yazdıran yani LEFT JOIN sorgusunu oluşturtan bir fonksiyondur.

#region Left Join
var query = from person in context.Persons
            join order in context.Orders
                on person.PersonId equals order.PersonId into personOrders  //personların orderlarını gurupladık burada
            from order in personOrders.DefaultIfEmpty()  //left joın yapmak ıcın normal degerı varsa getır dedık yoksa da default vea null ver demıs olduk burada
            select new
            {
                person.Name,
                order.Description
            };

var datas = await query.ToListAsync();

//BUNARIN methodsyntax yok 

#endregion

#region Right Join
//olusturulmaz aslında sadece yerlerını degıstırıcez ustek ıle efcore desteklemez right joini 
var query = from order in context.Orders
            join person in context.Persons
                on order.PersonId equals person.PersonId into orderPersons
            from person in orderPersons.DefaultIfEmpty()
            select new
            {
                person.Name,
                order.Description
            };

var datas = await query.ToListAsync();
#endregion

#region Full Join
//ful joın de yapılmaz sen kendın mantıgını kurmalısın 
var leftQuery = from person in context.Persons
                join order in context.Orders
                    on person.PersonId equals order.PersonId into personOrders
                from order in personOrders.DefaultIfEmpty()
                select new
                {
                    person.Name,
                    order.Description
                }; 


var rightQuery = from order in context.Orders
                 join person in context.Persons
                     on order.PersonId equals person.PersonId into orderPersons
                 from person in orderPersons.DefaultIfEmpty()
                 select new
                 {
                     person.Name,
                     order.Description
                 };

//uste sag ve sol ları aldık

var fullJoin = leftQuery.Union(rightQuery); //ıkısını bırlestırdık

var datas = await fullJoin.ToListAsync();
#endregion

#region Cross Join
//Cross Join işlemi ise iki tabloyu birleştirirken iki tablo arasında tüm eşleştirmeleri listeler yani çapraz birleştirir bir diğer tabir ile kartezyen çarpımını alır
var query = from order in context.Orders
            from person in context.Persons
            select new
            {
                order,
                person
            };

var datas = await query.ToListAsync();
#endregion

#region Collection Selector'da Where Kullanma Durumu

var query = from order in context.Orders
            from person in context.Persons.Where(p => p.PersonId == order.PersonId)
            select new
            {
                order,
                person
            };

var datas = await query.ToListAsync();
//burayı ınner joın gıbı algılar ef core 

#endregion

#region Cross Apply
//Inner Join -->Cross Apply'dan dönen sonuçları kolonları ile beraber diğer tablo içine transfer etmede kullanılır.

var query = from person in context.Persons
            from order in context.Orders.Select(o => person.Name)
            select new
            {
                person,
                order
            };

var datas = await query.ToListAsync();
#endregion

#region Outer Apply
//Left Join  -->output kümesinde sol tablodaki tüm satırlar ve bu satırların karşılık geldiği sağ tablodaki tüm satırlar bulunur.
var query = from person in context.Persons
            from order in context.Orders.Select(o => person.Name).DefaultIfEmpty()
            select new
            {
                person,
                order
            };

var datas = await query.ToListAsync();
#endregion
#endregion
Console.WriteLine();
public class Photo
{
    public int PersonId { get; set; }
    public string Url { get; set; }

    public Person Person { get; set; }
}
public enum Gender { Man, Woman }
public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }

    public Photo Photo { get; set; }
    public ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }

    public Person Person { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Photo>()
            .HasKey(p => p.PersonId);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Photo)
            .WithOne(p => p.Person)
            .HasForeignKey<Photo>(p => p.PersonId);

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost, 1433;Database=ApplicationDB;User ID=SA;Password=sıfre+!;TrustServerCertificate=True");
    }
}