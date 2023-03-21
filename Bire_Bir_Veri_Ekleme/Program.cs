// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
ApplicationDbContext context = new();
Console.WriteLine("Tamam");

#region One to one ılıskısel senaryolarda verı ekleme 

#region 1.Yöntem


// ısı ekleme
Person person=new Person();
person.Name = "Salih";
person.Address = new Address() { PersonAddress="Konya Meram" }; //burada kendısı adrres yerıne gıder ve orada Konya Meram yerını olusturur Id sını buraya getırır 


await context.AddAsync(person);
#endregion
//Egerkı prıncıpal entıty uzerınden ekleme gerceklestırılıyorsa dependent entıtysı verılmek zorunda degıldır  amma velakın dependent uzerınden ekelem ıslemı gerceklestırıllıyorsa eger burada prıncıpal entıty nesnesıne ıhtıyCIMIZ VARDIR

#region 2.Yöntem --> Dependent entıty uzerınden Principal entıty verısı ekleme
Address address = new() { 
PersonAddress="Konya Meram",
Person = new() { Name="Ali"}
};

await context.AddAsync(address);
await context.SaveChangesAsync();
#endregion



class Person  //Princitipal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; } //navıgatıon property 

}

class Address //Dependent
{
    public int Id { get; set; }
    public string PersonAddress { get; set; }
    public Person Person { get; set; }
}

class ApplicationDbContext : DbContext
{
    
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Save;User Id=SA ; Password=Viabelli34*.;TrustServerCertificate=true");

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
           .HasForeignKey<Address>(a => a.Id); //depenedent 
    }

}

#endregion


#region One to many ılıskısel senaryolarda verı ekleme 

#region 1. Yöntem -> Principal Entity Üzerinden Dependent Entity Verisi Ekleme

#region Nesne Referansı Üzerinden Ekleme
//nul durumlarında hata vere bılır burası
Blog blog = new() { Name="sadık.sunbul.com Blog"}; //bıre cok karssılıgındakı postlarımızı alta ekledık
blog.Posts.Add(new() { Title = "Post 1" });
blog.Posts.Add(new() { Title = "Post 2" });
blog.Posts.Add(new() { Title = "Post 3" });

await context.AddAsync(blog);
await context.SaveChangesAsync();
#endregion

#region Object Initializer Üzerinden Ekleme
Blog blog2 = new()
{
    Name = "A blog",
    Posts = new HashSet<Post>() { new Post() { Title="Post 4"}, new Post() { Title = "Post 5" } } //burada HasSet yerıne lıst de kullanıla bılır
};
await context.AddAsync(blog2);
await context.SaveChangesAsync();

#endregion

#endregion

#region 2. Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme
//bunu kulanmıycaz hıcbır zaman bu aykırıdır 1 e coga 

#endregion

#region 3. Yöntem -> Foreign Key Kolonu Üzerinden Veri Ekleme


//1. ve 2. yontemler hıc olmayan verılerın ılıskısel olarak eklenmesını saglarken bu 3. yontem onceden eklenmıs olan bır principal entıty verısıyle yenı dependent entıtylerı ılıskısel olarak eşleştirilmesini saglar
Post post1 = new()
{
    BlogId=1, //1. kullanıcıya post 7 yı eklemıs olduk 
    Title="Post 7"
};

await context.AddAsync(post1);
await context.SaveChangesAsync();

#endregion

class Blog
{   //burada her post 1 bloga baglı ıken her blok bırden fazla bloga baglıdır
    public Blog()
    {
        Posts = new HashSet<Post>(); //burada bunu yazmamızın sebebı null olma durumunu kaldırdık 
        //referans ıle yapıcaksak burayı yazarız 
        //burada bır nevı new leme yapmıs olduk 
        //burayı object de yapcaksak yazmaya gerek yok
    }
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public string Title { get; set; }

    public Blog Blog { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Save_;User Id=SA ; Password=Viabelli34*.;TrustServerCertificate=true");
    }
}

#endregion



#region May to many ılıskısel senaryolarda verı ekleme

#region 1. Yöntem
// n to n ilişkisi eğer ki default convention üzerinden tasarlanmışsa kullanılan bir yöntemdir.

Book book = new()
{
BookName = "A Kitabı",
Authors = new HashSet<Author>()
    {
        new Author(){ AuthorName = "Hilmi" },
        new(){ AuthorName = "Ayşe" },
        new(){ AuthorName = "Fatma" },
    }
};

await context.Books.AddAsync(book);
await context.SaveChangesAsync();



class Book
{
    public Book()
    {
        Authors = new HashSet<Author>(); //null hatasını almamak ıcın yazdık burayı
    }
    public int Id { get; set; }
    public string BookName { get; set; }

    public ICollection<Author> Authors { get; set; }
}

class Author
{
    public Author()
    {
        Books = new HashSet<Book>();//null hatasını almamak ıcın yazdık burayı
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }

    public ICollection<Book> Books { get; set; }
}
#endregion
#region 2. Yöntem
//n t n ilişkisi eğer ki fluent api ile tasarlanmışsa kullanılan bir yöntemdir.

Author author = new()
{
    AuthorName = "Mustafa",
    Books = new HashSet<AuthorBook>() {
        new(){ BookId = 1}, //1 ıd sıne sahıp olan kıtabı ılıskılendıre bılırız  yanı burada 1 ıd sıne sahıp olan kıtaba yenı bır yazar eklemıs olduk
        new AuthorBook(){ Book = new  Book() { BookName = "B Kitap" } } //burada olmayan bır kıtabı getırdık 
    }
};

await context.AddAsync(author);
await context.SaveChangesAsync();

class Book
{
    public Book()
    {
        Authors = new HashSet<AuthorBook>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }

    public ICollection<AuthorBook> Authors { get; set; }
}

class AuthorBook
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public Book Book { get; set; }
    public Author Author { get; set; }
}

class Author
{
    public Author()
    {
        Books = new HashSet<AuthorBook>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }

    public ICollection<AuthorBook> Books { get; set; }
}
#endregion


class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost, 1433;Database=ApplicationDb;User ID=SA;Password=sıfre+!");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorBook>()
            .HasKey(ba => new { ba.AuthorId, ba.BookId });

        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.Authors)
            .HasForeignKey(ba => ba.BookId);

        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Author)
            .WithMany(b => b.Books)
            .HasForeignKey(ba => ba.AuthorId);
    }
}
#endregion

