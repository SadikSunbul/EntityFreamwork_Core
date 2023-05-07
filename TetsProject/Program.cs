
using Microsoft.EntityFrameworkCore;
using System.Reflection;


DenemeContext context = new();


x x1 = new x() { Xaçıklama = " x içerisine yazıldı" };
y y1 = new y() { Xaçıklama = "y içerisinden yazıldı", Yaçıklama = "y içinden yazıldı" };
z z1 = new z() { Xaçıklama = "z içerisinden yazıldı", Yaçıklama = "z içerisinden yazıldı", Zaçıklama = "z içerisinden yazıldı" };
q q1 = new q() { Xaçıklama = "q içerisinden yazıldı", Qaçıkalam = "q içerisinden yazıldı" };

await context.AddRangeAsync(x1, y1, z1, q1);
await context.SaveChangesAsync();
Console.WriteLine("Temam");
Console.ReadLine();


class x
{
    public int Id { get; set; }
    public string Xaçıklama { get; set; }


}
class y:x
{
    public string Yaçıklama { get; set; }
}
class z:y
{
    public string Zaçıklama { get; set; }
}
class q:x
{
    public string Qaçıkalam { get; set; }

}


class DenemeContext:DbContext
{
    public DbSet<x> x { get; set; }
    public DbSet<z> z { get; set; }
    public DbSet<y> y { get; set; }
    public DbSet<q> q { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Tets;User Id=sa ; Password=Viabelli34*.;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<x>().UseTpcMappingStrategy();

    }
}


