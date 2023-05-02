



using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

EticaretContext context = new();

Console.WriteLine("Tamma");
Console.ReadLine();

class Çalışan
{
    public int Id { get; set; }
    public string İsim { get; set; }
    public string Soyisim { get; set; }
    public int ÇalışanAddresId { get; set; }
    public ÇalışanAdress ÇalışanAdress { get; set; }

}

class ÇalışanAdress
{
    public int Id { get; set; }
    public string Adres { get; set; }
    public Çalışan Çalışan { get; set; }
}

class EticaretContext:DbContext
{


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=Tets;User Id=sa ; Password=Viabelli34*.;TrustServerCertificate=true"
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}


class ÇalışanConfigration : IEntityTypeConfiguration<Çalışan>
{
    public void Configure(EntityTypeBuilder<Çalışan> builder)
    {
        builder.HasOne(i => i.ÇalışanAdress)
            .WithOne(i => i.Çalışan)
            .HasForeignKey<Çalışan>(i=>i.ÇalışanAddresId);
        }
}