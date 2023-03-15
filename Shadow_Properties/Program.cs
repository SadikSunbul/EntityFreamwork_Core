// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


ApplicationDbContext context = new();



#region Shadow Properties - Gölge Özellikler
//Entity sınıflarında fiziksel olarak tanımlanmayan/modellenmeyen ancak EF Core tarafından ilgili entity için var olan/var olduğu kabul edilen property'lerdir.
//Tabloda gösterilmesini istemediğimiz/lüzumlu görmediğimiz/entity instance'ı üzerinde işlem yapmayacağımız kolonlar için shadow propertyler kullanılabilir.
//Shadow property'lerin değerleri ve stateleri Change Tracker tarafından kontrol edilir.
#endregion

#region Foreign Key - Shadow Properties
//İlişkisel senaryolarda foreign key property'sini tanımlamadığımız halde EF Core tarafından dependent entity'e eklenmektedir. İşte bu shadow property'dir.

//var blogs = await context.Blogs.Include(b => b.Posts)
//    .ToListAsync();
//Console.WriteLine();

#endregion

#region Shadow Property Oluşturma
//Bir entity üzerinde shadow property oluşturmak istiyorsanız eğer Fluent API'ı kullanmanız gerekmektedir.
//        modelBuilder.Entity<Blog>()
//            .Property<DateTime>("CreatedDate")

//Blog blog=new Blog();
//blog.Name = "Sadık.blog";
//blog.Posts=new List<Post>() { new Post() { Title="C#", lastUpdated=true } };

//await context.Blogs.AddAsync(blog);
//await context.SaveChangesAsync();
//Console.WriteLine("Tamam");
#endregion

#region Shadow Property'e Erişim Sağlama
#region ChangeTracker İle Erişim
//Shadow property'e erişim sağlayabilmek için Change Tracker'dan istifade edilebilir.

var blog = await context.Blogs.FirstAsync(); //ilk blogu elde ettık

var createDate = context.Entry(blog).Property("CreatedDate"); //erısım gostermek ıcınde property yazcaz
Console.WriteLine(createDate.CurrentValue); //ın momerıdekı degerı 
Console.WriteLine(createDate.OriginalValue);  //data basede kı deger

createDate.CurrentValue = DateTime.Now;
await context.SaveChangesAsync();

#endregion

#region EF.Property İle Erişim
//Özellikle LINQ sorgularında Shadow Propery'lerine erişim için EF.Property static yapılanmasını kullanabiliriz.
//genelde burasını kullanıcaz 
var blogs = await context.Blogs.OrderBy(b => EF.Property<DateTime>(b, "CreatedDate")).ToListAsync();

var blogs2 = await context.Blogs.Where(b => EF.Property<DateTime>(b, "CreatedDate").Year > 2020).ToListAsync();
Console.WriteLine();





#endregion
#endregion



class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}

class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public string Title { get; set; }
    public bool lastUpdated { get; set; }

    public Blog Blog { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Shadow;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()  //blog ustunde bır ıslem yapıcaz 
            .Property<DateTime>("CreatedDate"); //blogun ıcerısındekı propertylerle ılgılı ıslem yapıcam dedk  --_> shadow ıcın generıgı tercıh edıcez olusturacagımız turu orda belırtcez olustrulma tarıhını tutucaz  ---> tabloda yenı bır yer acılıcak ve orada verıyı tutucak ama bızım fızıksel olarak bızım entıtymızde gozukmıycektır
        base.OnModelCreating(modelBuilder);
    }
}