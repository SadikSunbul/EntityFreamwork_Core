
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;


DenemeContext context = new();


//x x1 = new x() { Xaçıklama = " x içerisine yazıldı" };
//y y1 = new y() { Xaçıklama = "y içerisinden yazıldı", Yaçıklama = "y içinden yazıldı" };
//z z1 = new z() { Xaçıklama = "z içerisinden yazıldı", Yaçıklama = "z içerisinden yazıldı", Zaçıklama = "z içerisinden yazıldı" };
//q q1 = new q() { Xaçıklama = "q içerisinden yazıldı", Qaçıkalam = "q içerisinden yazıldı" };

//await context.AddRangeAsync(x1, y1, z1, q1);
//await context.SaveChangesAsync();
//Anne anne = new()
//{
//     İsim="Hasibe",
//      Çocuklar=new HashSet<Çocuk>() { new Çocuk() { 
//       İsim="Sadık"},
//       new Çocuk() {İsim="Ali"},
//       new Çocuk() {İsim="fatıma"}
//      }
//};

//Anne anne2 = new()
//{
//     İsim="Ayşe",
//      Çocuklar=new HashSet<Çocuk>()
//      {
//          new Çocuk(){ İsim="Taha"},
//          new Çocuk(){ İsim="Ahmet"}
//      }
//};
//context.Anneler.AddRange(anne,anne2);
//await context.SaveChangesAsync();


//var data =  context.Anneler.FirstOrDefault(i => i.Id == 2);

//    Console.WriteLine($"{data.İsim}");


//Console.WriteLine("Sonradan cocuklarının eklenmesi");

//await context.Entry(data).Collection(i => i.Çocuklar).LoadAsync();

//foreach (var item in data.Çocuklar)
//{
//    Console.WriteLine($"{data.İsim}-{data.Çocuklar.Count}-{item.İsim}");
//}

//var data= context.Anneler.FirstOrDefault(i => i.Id == 2);

//foreach (var a in data.Çocuklar)
//{
//    Console.WriteLine($"{data.İsim}-{a.İsim}");
//}

var query = from Anne in context.Anneler
            join Çocuk in context.Çocuklar
              on Anne.Id equals Çocuk.AnneId
            select new
            {
                Anne,
                Çocuk
            };
var data = await query.ToListAsync();

Console.WriteLine("Temam");
Console.ReadLine();


public class x
{
    public int Id { get; set; }
    public string Xaçıklama { get; set; }


}
public class y:x
{
    public string Yaçıklama { get; set; }
}
public class z:y
{
    public string Zaçıklama { get; set; }
}
public class q:x
{
    public string Qaçıkalam { get; set; }

}

public class Anne
{
    public int Id { get; set; }
    public string? İsim  { get; set; }
    
    public virtual ICollection<Çocuk> Çocuklar { get; set; }
}

public class Çocuk
{
    public int Id { get; set; }
    public string? İsim { get; set; }
    public int AnneId { get; set; }
    public virtual Anne Anne { get; set; }
}



public class DenemeContext:DbContext
{
    public DbSet<x> x { get; set; }
    public DbSet<z> z { get; set; }
    public DbSet<y> y { get; set; }
    public DbSet<q> q { get; set; }
    public DbSet<Anne> Anneler { get; set; }
    public DbSet<Çocuk> Çocuklar { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Tets;User Id=sa ; Password=Viabelli34*.;TrustServerCertificate=true");

        //optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<x>().UseTpcMappingStrategy();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasSequence("ec_sec")
            .StartsAt(990)
            .IncrementsBy(1);
    }
}

public class AnneConfigration : IEntityTypeConfiguration<Anne>
{
    public void Configure(EntityTypeBuilder<Anne> builder)
    {
        builder.HasMany(i => i.Çocuklar)
            .WithOne(i => i.Anne)
            .HasForeignKey(i=>i.AnneId);

        builder.HasIndex(i => i.İsim);

        builder.Property<int>("Rastgelesayi")
            .HasDefaultValueSql("NEXT VALUE FOR ec_sec");
    }
}


