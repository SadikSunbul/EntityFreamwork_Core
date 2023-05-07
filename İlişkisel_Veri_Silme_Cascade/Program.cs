// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
ApplicationDbContext context =new ApplicationDbContext();





#region One to one ilişkisel senaryolarda veri silme 

Person? person = await context.Persons.Include(i=>i.Address).FirstOrDefaultAsync(i=>i.Id==1); //adrses bılgısıyle aldık verıyı 

if (person != null) //null degılse
{
    context.Addresses.Remove(person.Address);
}
context.SaveChanges();

#endregion


#region One to many ilişkisel senaryolarda veri silme

Blog? blog = await context.Blogs
    .Include(i => i.Posts)
    .FirstOrDefaultAsync(i=>i.Id==1);

Post post=blog.Posts.FirstOrDefault(i => i.Id == 2);

context.Posts.Remove(post);
context.SaveChanges();

#endregion


#region Many to many ilişkisel senaryolarda veri silme

//burada bır kıtabın yazarını sılmek hatalı olucaktır o yazarın baska kıtaplarıda olabılır o yuzden hatalı b rıs yapmıs oluruz o yuzde crose tablede sılıcez

Book? book = await context.Books.Include(i => i.Authors).FirstOrDefaultAsync(i => i.Id == 1); //kıtap ve tum yazarlar elde edıldı
Author? author=book.Authors.FirstOrDefault(i => i.Id == 2); //2 ıd sınıe sahıp olan yazarını bulduk
//context.Authors.Remove(author); //boyle yaparsan drekt yazarı sılersın bu degıl bızım amacımız
book.Authors.Remove(author); //bu kıtabın yazarlarından uyusanı sıler asıl amacımız bu --> crose tablede sılınmesı gerektıgını anlar efcore

context.SaveChanges();

#endregion


#region Cascade Delete
//bu davranıs modellero fullent ıp ıle confıgre edılmektedır.
#region Cascade
//esas tablodan prıncıpal dan sılınen verı ıle karsı bagımlı tablodan bulunan ılısklı verılerın sılınmesını saglar 
/*
Blog blog=await context.Blogs.FindAsync(1);
context.Blogs.Remove(blog);
await context.SaveChangesAsync(); //buna karsılık ılıskısel verı tablonunda ne varsa onlarıda sılıcektır
*/
#endregion

#region SettNull
//esas tablodan prıncıpal dan sılınen verı ıle karsı bagımlı tablodan bulunan ılısklı verılere null degerın atanmasını saglar 
//bıre bırde senaryolarda egerkı foren key ve prımary key kolonları aynı ıse setnull davranısını kullanamayız null alamaz burada pk kolonu ---->forenkey kolonunu null yapıyor ztn 
/*
Blog blog = await context.Blogs.FindAsync(1);
context.Blogs.Remove(blog);
await context.SaveChangesAsync(); //burada null yapıcaktır ustekı gıbı sılmez
*/
#endregion

#region Restrict
//esas tablodan herhangı bı verı sılınmeye calısıldıgında o verıye karsılık dependent verıye karsılık dependent table'da ilişkisel veriler varsa eger bu sılme ıslemını engelemesını saglar
//bunu sılemeyız
Blog blog = await context.Blogs.FindAsync(1);
context.Blogs.Remove(blog);
await context.SaveChangesAsync();

#endregion

#endregion


#region Saving Data
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


#region Claslar


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
    public int? BlogId { get; set; } //set null kullanıla bılırız
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
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=VeriSilme;User Id=SA ; Password=sıfre*.;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
        //burada personda bır verı sılınırse adreslerdekı ılıskılı olanıda sıl dedık

        //default olarak cascade dır bunlar ztn

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .IsRequired(false) //burda ılgılı forenkey colonu ıllakı requert olmak zorunda degıl 
            .OnDelete(DeleteBehavior.SetNull); //principal dan veri silindiğinde

        modelBuilder.Entity<Book>()   //coka cokta belırlıyemeyız required i
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books);
    }
}

#endregion

