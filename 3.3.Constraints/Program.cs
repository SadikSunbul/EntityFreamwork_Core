using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

ApplicationDbContext context = new();
/*
 HasKey yöntemi, birincil anahtar (primary key) için kullanılırken, HasAlternateKey yöntemi, benzersiz ama birincil anahtar (primary key) olmayan bir özellik veya özellik grubu için kullanılır.
 */
//IsUnique yöntemi kullanılarak bu özelliğin benzersiz olması sağlanır.

/*
 modelBuilder.Entity<Musteri>()
            .HasIndex(m => m.Email)
            .IsUnique();  //Tavsıye edılen 
        //bunun ıle sunun arasındakı fark ne 
        modelBuilder.Entity<Musteri>()
            .HasIndex(i=>i.Email)
            .HasAlternateKey(i=>i.Email)
            ;
 */

//burada sadece baslıklar yazıldı cunkı bu konuları oncekı derslerde ısledıgımız ıcın sadece ana baslıkları koyduk buraya 
/*
 * Alternate keys (alternatif anahtarlar) ve normal keys (standart anahtarlar) veritabanı tasarımında kullanılan iki farklı anahtar türüdür. Her iki anahtar türü de bir tablodaki her satırın benzersiz bir şekilde tanımlanmasına yardımcı olur.

Normal anahtarlar (veya Primary Keys) bir tablonun benzersiz satırlarının tanımlanmasında kullanılır. Bu anahtar türü, bir satırın diğer satırlardan farklılaşmasını sağlayan tek bir alanı veya birleşik bir alan grubunu kullanır. Normal anahtarlar, bir tablodaki her satırın benzersiz bir şekilde tanımlanmasını sağlar ve diğer tablolarla ilişkilendirme yapmak için kullanılabilir.

Alternatif anahtarlar, normal anahtarların yanı sıra bir tablodaki satırların benzersiz bir şekilde tanımlanmasına yardımcı olan başka bir anahtar türüdür. Alternatif anahtarlar, bir tabloda birden fazla alanın birleşimini kullanarak satırları benzersiz olarak tanımlayabilir. Normal anahtarlar, bir tablonun benzersiz satırlarının tanımlanmasında kullanılan tek anahtardırken, bir tabloda birden fazla alternatif anahtar bulunabilir.

Alternatif anahtarlar, normal anahtarlarla birlikte kullanıldığında bir veritabanının performansını artırabilir. Örneğin, bir sorgu normal anahtara dayanarak yapıldığında, performans yüksek olabilir ancak alternatif anahtarlarla yapılan sorgular da normal anahtara göre daha iyi sonuçlar verebilir. Alternatif anahtarlar, normal anahtarlar gibi benzersiz tanımlama sağladıkları için, bir tablodaki verilerin doğruluğunu da artırabilirler.
 */
#region Primary Key Constraint

//Bir kolonu PK constraint ile birincil anahtar yapmak istiyorsak eğer bunun için name convention'dan istifade edebiliriz. Id, ID, EntityNameId, EntityNameID şeklinde tanımlanan tüm propertyler default olarak EF Core tarafından pk constraint olacak şekilde generate edilirler.
//Eğer ki, farklı bir property'e PK özelliğini atamak istiyorsan burada HasKey Fluent API'ı yahut Key attribute'u ile bu bildirimi iradeli bir şekilde yapmak zorundasın.

#region HasKey Fonksiyonu

#endregion
#region Key Attribute'u

#endregion
#region Alternate Keys - HasAlternateKey
//Bir entity içerisinde PK'e ek olarak her entity instance'ı için alternatif bir benzersiz tanımlayıcı işlevine sahip olan bir key'dir.
#endregion
#region Composite Alternate Key

#endregion

#region HasName Fonksiyonu İle Primary Key Constraint'e İsim Verme

#endregion
#endregion

#region Foreign Key Constraint

#region HasForeignKey Fonksiyonu

#endregion
#region ForeignKey Attribute'u

#endregion
#region Composite Foreign Key

#endregion

#region Shadow Property Üzerinden Foreign Key

#endregion

#region HasConstraintName Fonksiyonu İle Primary Key Constraint'e İsim Verme

#endregion
#endregion

#region Unique Constraint --> Tekrarı engeller

#region HasIndex - IsUnique Fonksiyonları

#endregion

#region Index, IsUnique Attribute'ları

#endregion

#region Alternate Key

#endregion
#endregion

#region Check Constratint

#region HasCheckConstraint

#endregion
#endregion

//[Index(nameof(Blog.Url), IsUnique = true)]
class Blog
{
    public int Id { get; set; }
    public string BlogName { get; set; }
    public string Url { get; set; }

    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    //public int BlogId { get; set; }
    public string Title { get; set; }
    public string BlogUrl { get; set; }
    public int A { get; set; }
    public int B { get; set; }

    public Blog Blog { get; set; }
}


class ApplicationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasKey(b => b.Id)
            .HasName("ornek");
        modelBuilder.Entity<Blog>()
            .HasAlternateKey(b => new { b.Url, b.BlogName });
        modelBuilder.Entity<Blog>()
            .Property<int>("BlogForeignKeyId");

        modelBuilder.Entity<Blog>()
            .HasMany(b => b.Posts)
            .WithOne(b => b.Blog)
            .HasForeignKey("BlogForeignKeyId")
            .HasConstraintName("ornekforeignkey");

        modelBuilder.Entity<Blog>()
            .HasIndex(b => b.Url)
            .IsUnique();             //burad Eşsiz yaptık altakı HasAlternateKey de aynı sekılde Eşsiz yapar
        modelBuilder.Entity<Blog>()
            .HasAlternateKey(b => b.Url);

        modelBuilder.Entity<Post>()
            .HasCheckConstraint("a_b_check_const", "[A] > [B]");   //a b den buyukse kayıtları kabul et  
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost, 1433;Database=ApplicationDB;User ID=SA;Password=sıfre+!;TrustServerCertificate=True");
    }
}


/*
 modelBuilder.Entity<Musteri>()
            .HasKey(m => m.Id); // Id alanını PK olarak belirle

        modelBuilder.Entity<Musteri>()
            .HasIndex(m => m.Email)
            .IsUnique(); // Email alanını benzersiz olarak ayarla

        base.OnModelCreating(modelBuilder);

Yukarıdaki kod bloğunda HasIndex metodu kullanarak Email özelliğine bir indeks ekliyoruz. IsUnique metodu ise bu indeksin benzersiz olmasını sağlıyor. Bu sayede, aynı email adresine sahip iki müşteri kaydedemeyeceğiz.

HasKey metodunu kullanarak Id alanını PK olarak belirledik. Bu sayede, her kaydın benzersiz bir Id değerine sahip olacağı garanti edildi.
 */