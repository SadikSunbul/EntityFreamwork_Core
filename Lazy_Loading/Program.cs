
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

ApplicationDbContext context = new();

// BURAA 4 OENMLI HUSUS VAR
/*
    1->Microsoft.EntityFrameworkCore.Proxies  İNDİR
    2-> OnConfiguring .UseLazyLoadingProxies() ekle
    3-> baglantı yerlerındekı propertylerı vırtual olarak ısaretle
    4-> clasların public olmasına dikkat et
 */
#region Lazy Loading Nedir?
//Navigation property'ler üzerinde bir işlem yapılmaya çalışıldığı taktirde ilgili propertynin/ye temsil ettiği/karşılık gelen tabloya özel bir sorgu oluşturulup execute edilmesini ve verilerin yüklenmesini sağlayan bir yaklaşımdır.
#endregion

var employee = await context.Employees.FindAsync(2);
Console.WriteLine(employee.Region.Name); //burada hata verıcektır bu kod bunun hata vermemesı ıcın Microsoft.EntityFrameworkCore.Proxies ındırıp onConfigrattions dan bellı ıslemler yapmamız gerekır bunları yptıgımız zaman artık boyle bır deger ıstedıgımızde lazerLoading sql sorgusunu gonderır  kendısı ve bıze degerı dondurur


#region Prox'lerle Lazy Loading
//Microsoft.EntityFrameworkCore.Proxies
//bunu yuklemek gerekır 

#region Property'lerin virtual Olması
//Eğer ki proxler üzerinden lazy loading operasyonu gerçekleştiriyorsanız Navigtation Propertylerin virtual ile işaretlenmiş olması gerekmektedir. Aksi taktirde patlama meydana gelecektir.
#endregion
#endregion

#region Proxy Olmaksızın Lazy Loading
//Prox'ler tüm platformlarda desteklenmeyebilir. Böyle bir durumda manuel bir şekilde lazy loading'i uygulamak mecburiyetinde kalabiliriz.

//Manuel yapılan Lazy Loading operasyonlarında Navigation Proeprtylerin virtual ile işaretlenmesine gerek yoktur!

#region ILazyLoader Interface'i İle Lazy Loading
//Microsoft.EntityFrameworkCore.Abstractions
//yuklemek gerekır ustekını

//var employee = await context.Employees.FindAsync(2);
#endregion
#region Delegate İle Lazy Loading
//var employee = await context.Employees.FindAsync(2);
#endregion
#endregion

#region N+1 Problemi
//var region = await context.Regions.FindAsync(1);
//foreach (var employee in region.Employees)
//{
//    var orders = employee.Orders;
//    foreach (var order in orders)
//    {
//        Console.WriteLine(order.OrderDate);
//    }
//}
#endregion

//Lazy Loading, kullanım açısından oldukça maliyetli ve performans düşürücü bir etkiye sahip yöntemdir. O yüzden kullanırken mümkün mertebe dikkatli olmalı ve özellikle navigation propertylerin döngüsel tetiklenme durumlarında lazy loading'i tercih etmemeye odaklanmalıyız. Aksi taktirde her bir tetiklemeye karşılık aynı sorguları üretip execute edecektir. Bu durumu N+1 Problemi olarak nitelendirmekteyiz.
//Mümkün mertebe, ilişkisel verileri eklerken Lazy Loading kullanmamaya özen göstermeliyiz.

Console.WriteLine();

#region Proxy İle Lazy Loading
public class Employee
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
    public virtual List<Order> Orders { get; set; }
    public virtual Region Region { get; set; }
}
public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
}
public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public virtual Employee Employee { get; set; }
}
#endregion
#region ILazyLoader Interface'i İle Lazy Loading
//Manuel yapımı 
//public class Employee
//{
//    ILazyLoader _lazyLoader;
//    Region _region;
//    public Employee() { }
//    public Employee(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public List<Order> Orders { get; set; }
//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region);
//        set => _region = value;
//    }
//}
//public class Region
//{
//    ILazyLoader _lazyLoader;
//    ICollection<Employee> _employees;
//    public Region() { }
//    public Region(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
//}
//public class Order
//{
//    public int Id { get; set; }
//    public int EmployeeId { get; set; }
//    public DateTime OrderDate { get; set; }
//    public Employee Employee { get; set; }
//}

#endregion
#region Delegate İle Lazy Loading
//public class Employee
//{
//    Action<object, string> _lazyLoader;
//    Region _region;
//    public Employee() { }
//    public Employee(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public List<Order> Orders { get; set; }
//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region);
//        set => _region = value;
//    }
//}
//public class Region
//{
//    Action<object, string> _lazyLoader;
//    ICollection<Employee> _employees;
//    public Region() { }
//    public Region(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
//}
//public class Order
//{
//    public int Id { get; set; }
//    public int EmployeeId { get; set; }
//    public DateTime OrderDate { get; set; }
//    public Employee Employee { get; set; }
//}

//static class LazyLoadingExtension
//{
//    public static TRelated Load<TRelated>(this Action<object, string> loader, object entity, ref TRelated navigation, [CallerMemberName] string navigationName = null)
//    {
//        loader.Invoke(entity, navigationName);
//        return navigation;
//    }
//}
#endregion

class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Region> Regions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer("Server=localhost, 1433;Database=ApplicationDB;User ID=SA;Password=sıfre+!;TrustServerCertificate=True");

        //optionsBuilder.UseLazyLoadingProxies();  //boylede cagrılır 
    }
}