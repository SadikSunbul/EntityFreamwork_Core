﻿


using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

ApplicationDbContext context = new();
var datas = await context.Persons.ToListAsync();
#region Neden Loglama Yaparız?
//Çalışan bir sistemin runtime'da nasıl davranış gerçekleştirdiğini gözlemleyebilmek için log mekanizmalarından istifade ederiz.
#endregion
#region Neleri Loglarız?
//Yapılan sorguların çalışma süreçlerindeki davranışları.
//Gerekirse hassas veriler üzerinde de loglama işlemleri gerçekleştiriyoruz.
#endregion
#region Basit Olarak Loglama Nasıl Yapılır?
//Minumum yapılandırma gerektirmesi.
//Herhangi bir nuget paketine ihtiyaç duyulmaksızın loglamanın yapılabilmesi.
/*
 En çok kullanılan log kaydı tutma sistemleri arasında NLog, Log4Net ve Serilog yer almaktadır. Bu sistemler geniş bir kullanıcı kitlesi tarafından benimsenmiş ve aktif olarak geliştirilmektedir.

NLog, .NET Framework ve .NET Core uygulamaları için açık kaynak kodlu bir loglama kütüphanesidir. Basit yapılandırma dosyalarıyla kolayca yapılandırılabilir ve esnek bir şekilde özelleştirilebilir.

Log4Net de açık kaynaklı bir log kaydı tutma sistemi olup, NLog gibi basit yapılandırma dosyaları kullanarak yapılandırılabilir ve özelleştirilebilir.

Serilog, .NET uygulamaları için açık kaynaklı bir loglama kütüphanesidir. NLog ve Log4Net'den farklı olarak, kod ve yapılandırma dosyaları arasında daha az ayrım yapar ve üzerinde çalışmanın daha kolay olduğu bir API sağlar.

Ancak en çok kullanılan loglama sistemi her zaman ihtiyaca bağlı olarak değişebilir. Projenin gereksinimlerine ve tercihlerinize bağlı olarak, farklı loglama sistemleri sizin için daha uygun olabilir.
 */
#region Debug Penceresine Log Nasıl Atılır?

#endregion
#region Bir Dosyaya Log Nasıl Atılır?
//Normalde console yahut debug pencerelerine atılan loglar pek takip edilebilir nitelikte olmamaktadır.
//Logları kalıcı hale getirmek istediğimiz durumlarda en basit halyile bu logları harici bir dosyaya atmak isteyebiliriz.
#endregion

#endregion
#region Hassas Verilerin Loglanması - EnableSensitiveDataLogging
//Default olarak EF Core log mesajlarında herhangi bir verinin değerini içermemektedir. Bunun nedeni, gizlilik teşkil edebilecek verilerin loglama sürecinde yanlışlıklada olsa açığa çıkmamasıdır. 
//Bazen alınan hatalarda verinin değerini bilmek hatayı debug edebilmek için oldukça yardımcı olabilmektedir. Bu durumda hassas verilerinde loglamasını sağlayabiliriz.
#endregion
#region Exception Ayrıntısını Loglama - EnableDetailedErrors

#endregion
#region Log Levels
//EF Core default olarak Debug sevisinin üstündeki(debug dahil) tüm davranıuşları loglar.
#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }

    public ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

    public Person Person { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }

    StreamWriter _log = new("logs.txt", append: true); //uygulamanın debug klasorune bu yoksa  klasoru olusturur varsa  append: true uzerıne ekleme
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost, 1433;Database=ApplicationDB;User ID=SA;Password=1q2w3e4r+!;TrustServerCertificate=True");

        //optionsBuilder.LogTo(Console.WriteLine);
        //optionsBuilder.LogTo(message => Debug.WriteLine(message));
        optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message), LogLevel.Information)  //LogLevel.Information OZELLIK SEVIYE OZELLIGI GIBI FILTRELEME YAPARIZ 
            .EnableSensitiveDataLogging() //verılerıde loglar
            .EnableDetailedErrors();  //haTAYI DETAYLANDIRIR
        //optionsBuilder.LogTo(message => _log.WriteLine(message));
    }

    public override void Dispose() // Dispose --->Elden çıkarmak
    {
        base.Dispose();
        _log.Dispose(); //logu kapadık 
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _log.DisposeAsync();
    }
}
