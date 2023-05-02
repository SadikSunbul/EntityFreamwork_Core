
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

DenemeContext context = new DenemeContext();

//Çalışan calısan1 = new()
//{
//    Adi = "Sadık",
//    Departman = new Departman()
//    {
//        DepartmanAdi = "Backend"
//    }
//};
//Çalışan calısan2 = new()
//{
//    Adi = "Ali",
//    DepartmanId = 1
//};

//Çalışan calısan3 = new()
//{
//    Adi = "Hasan",
//    Departman = new Departman()
//    {
//        DepartmanAdi = "Frontend"
//    }
//};

//Departman departman1 = new()
//{
//    DepartmanAdi = "Sekreter"
//};
//await context.Departmanlar.AddAsync(departman1);
//await context.Çalışanlar.AddRangeAsync(calısan1, calısan2, calısan3);

var data = await context.Çalışanlar.Include(i => i.Departman).FirstOrDefaultAsync(i => i.Id == 1);

Departman? departaman = await context.Departmanlar.FirstOrDefaultAsync(i => i.DepartmanAdi == "Frontend");



data.Departman = departaman;

await context.SaveChangesAsync();
Console.WriteLine("Temam");
Console.ReadLine();

class Çalışan 
{
    public int Id { get; set; }
    public string Adi { get; set; }
    public int DepartmanId { get; set; }
    public Departman Departman { get; set; }
}

class Departman
{
    public int Id { get; set; }
    public string DepartmanAdi { get; set; }
    public ICollection<Çalışan> Çalışanlar { get; set; }
}

class DenemeContext:DbContext
{
    public DbSet<Çalışan> Çalışanlar { get; set; }
    public DbSet<Departman> Departmanlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Tets;User Id=sa ; Password=Viabelli34*.;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}


class ÇalşanConfigartion : IEntityTypeConfiguration<Çalışan>
{
    public void Configure(EntityTypeBuilder<Çalışan> builder)
    {
        builder.HasOne(i => i.Departman)
        .WithMany(i => i.Çalışanlar)
        .HasForeignKey(i => i.DepartmanId);
            
    }
}

