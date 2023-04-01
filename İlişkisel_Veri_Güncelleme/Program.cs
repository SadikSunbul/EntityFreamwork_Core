// See https://aka.ms/new-console-template for more information


using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

ApplicationDbContext context = new();

#region One to One İlişkisel Senaryolarda Veri Güncelleme
#region Saving
//burada verı ekledık 
//Person person = new()
//{
//    Name = "Gençay",
//    Address = new()
//    {
//        PersonAddress = "Yenimahalle/ANKARA"
//    }
//};

//Person person2 = new()
//{
//    Name = "Hilmi"
//};

//await context.AddAsync(person);
//await context.AddAsync(person2);
//await context.SaveChangesAsync();
#endregion

#region 1. Durum | Esas tablodaki veriye bağımlı veriyi değiştirme

Person? person = await context.Persons 
    .Include(p => p.Address)//burada adres bılgılerını buradan elde ettık --> burada vermıs  oldugumuz navıgatıon property ıle arkada olusturacagı select sorgusunda  joın ıslemı yapar --> burası nulll olabılır o yuzden ? koyduk
    .FirstOrDefaultAsync(p => p.Id == 1); //person ıd sı 1 olanı elde ettık 

context.Addresses.Remove(person.Address); //burada person ıcerısındekı adresı sıl dedık guncelleme ıcın ılk once sılmek gerekır
person.Address = new Address() //yenı bır nesne olusturduk
{
    PersonAddress = "Yeni adres" //yenı adresı buraya gırdık
};

await context.SaveChangesAsync(); //kaydettık burada

#endregion
#region 2. Durum | Bağımlı verinin ilişkisel olduğu ana veriyi güncelleme

Address? address = await context.Addresses.FindAsync(1); //adres bılgısını elde ettık ıd sı 1 olanı
address.Id = 2; //bu adresın ıd sını 2 olarak verdık hata verıcektır
await context.SaveChangesAsync();


Address? address = await context.Addresses.FindAsync(2); //burada 1. adresı aldık
context.Addresses.Remove(address); //sıldık
await context.SaveChangesAsync(); //kaydettık

Person? person = await context.Persons.FindAsync(2); //hılmıyı aldık 
address.Person = person; // sılınenı bunnunla ılıskılendır bunu ustekı sılınen person ıle ılıskılendır dedık

address.Person = new() //yenı bır adres 
{
    Name = "Rıfkı"
};
await context.Addresses.AddAsync(address); //adresı kaydetı yukarıdakı ılıskılendırmeyıde kaydettı -->burada adresı degıstırdıgımızı anlamaz cunkı oceden sılmıstık bunu

await context.SaveChangesAsync();


#endregion
#endregion

#region One to Many İlişkisel Senaryolarda Veri Güncelleme
#region Saving
//Blog blog = new()
//{
//    Name = "Gencayyildiz.com Blog",
//    Posts = new List<Post>
//    {
//        new(){ Title = "1. Post" },
//        new(){ Title = "2. Post" },
//        new(){ Title = "3. Post" },
//    }
//};

//await context.Blogs.AddAsync(blog);
//await context.SaveChangesAsync();
#endregion

#region 1. Durum | Esas tablodaki veriye bağımlı verileri değiştirme

Blog? blog = await context.Blogs
    .Include(b => b.Posts)
    .FirstOrDefaultAsync(b => b.Id == 1);

Post? silinecekPost = blog.Posts.FirstOrDefault(p => p.Id == 2);
blog.Posts.Remove(silinecekPost);

blog.Posts.Add(new() { Title = "4. Post" });
blog.Posts.Add(new() { Title = "5. Post" });

await context.SaveChangesAsync();

#endregion

#region 2. Durum | Bağımlı verilerin ilişkisel olduğu ana veriyi güncelleme
Post? post = await context.Posts.FindAsync(4);
post.Blog = new()
{
    Name = "2. Blog"
};
await context.SaveChangesAsync();


Post? post = await context.Posts.FindAsync(5);
Blog? blog = await context.Blogs.FindAsync(2);
post.Blog = blog;
await context.SaveChangesAsync();
#endregion
#endregion

#region Many to Many İlişkisel Senaryolarda Veri Güncelleme
#region Saving
//Book book1 = new() { BookName = "1. Kitap" };
//Book book2 = new() { BookName = "2. Kitap" };
//Book book3 = new() { BookName = "3. Kitap" };

//Author author1 = new() { AuthorName = "1. Yazar" };
//Author author2 = new() { AuthorName = "2. Yazar" };
//Author author3 = new() { AuthorName = "3. Yazar" };

//book1.Authors.Add(author1);
//book1.Authors.Add(author2);

//book2.Authors.Add(author1);
//book2.Authors.Add(author2);
//book2.Authors.Add(author3);

//book3.Authors.Add(author3);

//await context.AddAsync(book1);
//await context.AddAsync(book2);
//await context.AddAsync(book3);
//await context.SaveChangesAsync();
#endregion

#region 1. Örnek


Book? book = await context.Books.FindAsync(1);
Author? author = await context.Authors.FindAsync(3);
book.Authors.Add(author);

await context.SaveChangesAsync();


#endregion
#region 2. Örnek

//Author? author = await context.Authors
//    .Include(a => a.Books)
//    .FirstOrDefaultAsync(a => a.Id == 3); //3. yazarın bılgılerını aldık burada

//foreach (var book in author.Books) //burada yazarın kıtaplarını donduk
//{
//    if (book.Id != 1) //ıd sı 1 olan harıc dıgerlerınde gır dedık
//    {
//        author.Books.Remove(book); //yapıyı crose tableden sıler burda da sıldık
//        //burada kıtap yerınde 2 3 4 ıd sındekı kıtaplar sılınmez onlar orada durur
//    }
//}

await context.SaveChangesAsync();

#endregion
#region 3. Örnek

Book? book = await context.Books
    .Include(b => b.Authors)
    .FirstOrDefaultAsync(b => b.Id == 2);

Author silinecekYazar = book.Authors.FirstOrDefault(a => a.Id == 1);
book.Authors.Remove(silinecekYazar);

Author author = await context.Authors.FindAsync(3);
book.Authors.Add(author);
book.Authors.Add(new() { AuthorName = "4. Yazar" });

await context.SaveChangesAsync();
  
#endregion
#endregion


class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Address Address { get; set; }
}
class Address
{
    public int Id { get; set; }
    public string PersonAddress { get; set; }

    public Person Person { get; set; }
}
class Blog
{
    public Blog()
    {
        Posts = new HashSet<Post>();
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
class Book
{
    public Book()
    {
        Authors = new HashSet<Author>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }

    public ICollection<Author> Authors { get; set; }
}
class Author
{
    public Author()
    {
        Books = new HashSet<Book>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }

    public ICollection<Book> Books { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Test5;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
    }
}
