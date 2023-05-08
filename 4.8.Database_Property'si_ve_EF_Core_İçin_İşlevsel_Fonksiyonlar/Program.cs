
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Transactions;

ApplicationDbContext context = new();
#region Database Property'si
//Database property'si veritabanını temsil eden ve EF Core'un bazı işlevlerinin detaylarına erişmemizi sağlayan bir propertydir.
#endregion
#region BeginTransaction  ->İşleme Başla demek 
//EF Core, transaction yönetimini otomatik bir şekilde kendisi gerçekleştirmektedir. Eğer ki transaction yönetimini manuel olarak anlık ele almak istiyorsak BeginTransaction fonksiyonunu kullanabiliriz.

//IDbContextTransaction transaction = context.Database.BeginTransaction();
/*
 Arayüz IDbContextTransaction, Entity Framework Core tarafından yönetilen bir işlemi temsil eder. İşlem içinde yapılan değişiklikleri taahhüt etmek veya geri almak gibi işlemi kontrol etmek için yöntemler sağlar.
 */
/*
 Örneğin, bir banka uygulamasında para transferi işlemi yaparken, gönderen hesaptan para çıkışı yapılacak ve aynı zamanda alıcı hesaba para yatırılacaktır. İşlem başlatıldığında, bu iki değişiklik tek bir işlem olarak ele alınabilir ve başarısızlık durumunda her ikisi de geri alınabilir. BeginTransaction yöntemi, bu tür işlemleri yönetmek için oldukça faydalıdır.
 */
/*
 using var transaction = context.Database.BeginTransaction();

try
{
    // Perform database operations here...

    context.SaveChanges(); //verıler kaydedılırken hata varmı yokmu onu kontrol eder

    transaction.Commit(); //Ancak eğer hatalar oluşmadan işlem tamamlanırsa, commit() yöntemi çağrılır ve işlem sonlandırılır:
}
catch (Exception)
{
    transaction.Rollback(); //Eğer herhangi bir hata meydana gelirse, önceki tüm değişiklikler geri alınır ve işlem iptal edilir:
}
 */
/*
 Bu örnekte usingifade, işlemin artık ihtiyaç kalmadığında elden çıkarılmasını sağlar. Blok, tryişlem içinde gerçekleştirilen veritabanı işlemlerini içerir. Bir istisna atılırsa, Rollbackişlem içinde yapılan değişiklikleri geri almak için yöntem çağrılır. Aksi takdirde, Commitdeğişiklikleri veritabanına kaydetmek için yöntem çağrılır.
 */
#endregion
#region CommitTransaction
//EF Core üzerinde yapılan çalışmaların commit->işlemek edilebilmesi için kullanılan bir fonksiyondur.
//depolama sisteminde kalıcı değişiklikler yapma eylemini ifade eder.

//context.Database.CommitTransaction();
//begınden de yapılır bu Transaction lar
#endregion
#region RollbackTransaction
//EF Core üzerinde yapılan çalışmaların rollback edilebilmesi için kullanılan bir fonksiyondur.
//Bir işlemin parçası olarak gerçekleştirilen bir dizi veritabanı işlemini geri alma eylemini ifade eder.
//Bir veritabanı işleminde, eklemeler, güncellemeler ve silmeler gibi çoklu veritabanı işlemleri tek bir mantıksal birim olarak yürütülür. İşlemlerden herhangi biri sırasında bir hata oluşursa, işlemin tamamı geri alınabilir, yani işlem sırasında yapılan tüm değişiklikler geri alınır ve işlem başlamadan önce veritabanı orijinal durumuna geri döner.
//context.Database.RollbackTransaction();
//begınden de yapılır bu Transaction lar
#endregion
#region CanConnect
//Verilen connection string'e karşılık bağlantı kurulabilir bir veritabanı var mı yok mu bunun bilgisini bool türde veren bir fonksiyondur.
//bool connect = context.Database.CanConnect();
//Console.WriteLine(connect);
//Örneğin, bir kullanıcı bilgisayarından bir veritabanına erişmeye çalışabilir ve uygulama, veritabanı sunucusuyla bağlantı kurmaya çalışır. Bağlantı kurulmadan önce, uygulamanın veritabanına erişmek için gerekli kimlik bilgilerine ve izinlere sahip olup olmadığını belirlemesi gerekir. "CanConnect" işlevi, veritabanı bağlantısını test etmek ve gerekli kimlik bilgilerinin doğru olup olmadığını belirlemek için kullanılabilir.
//mıgratıons olusturmazsan false doner
#endregion
#region EnsureCreated
//EF Core'da tasarlanan veritabanını migration kullanmaksızın, runtime'da yani kod üzerinde veritabanı sunucusuna inşa edebilmek için kullanılan bir fonksiyondur.
//context.Database.EnsureCreated();
//ureCreatedveritabanı ve tabloları yoksa bunları oluşturan basit ve hafif bir yaklaşımdır. Veritabanının salt okunur işlemler için kullanıldığı veya veritabanının sık sık güncellenmesinin beklenmediği senaryolarda kullanılması amaçlanmıştır. EnsureCreatedveritabanı şemasındaki değişiklikleri veya veri geçişlerini işlemek için tasarlanmamıştır.
#endregion
#region EnsureDeleted
//İnşa edilmiş veritabanını runtime'da silmemizi sağlayan bir fonksiyondur.
//context.Database.EnsureDeleted();
#endregion
#region GenerateCreateScript
//Komut Dosyası Oluştur
//Context nesnesinde yapılmış olan veritabanı tasarımı her ne ise ona uygun bir SQL Script'ini string olarak veren metottur.
//var script = context.Database.GenerateCreateScript();
//Console.WriteLine(script);
/*
 GenerateCreateScript()yöntemi, bir SQL Server veritabanı nesnesinin (tablo, saklama prosedürü, vb.) CREATE script'ini oluşturmak için kullanılır. Bu yöntem, SQL Server Management Studio gibi araçlarda olduğu gibi, veritabanındaki bir nesnenin kodunu almanızı sağlar.
 */
#endregion
#region ExecuteSql
//Veritabanına yapılacak Insert, Update ve Delete sorgularını yazdığımız bir metottur. Bu metot işlevsel olarak alacağı parametreleri SQL Injection saldırılarına karşı korumaktadır. 
//string name = Console.ReadLine();
//var result = context.Database.ExecuteSql($"INSERT Persons VALUES('{name}')");
#endregion
#region ExecuteSqlRaw
//Veritabanına yapılacak Insert, Update ve Delete sorgularını yazdığımız bir metottur. Bu metotta ise sorguyu SQL Injection saldırılarına karşı koruma görevi geliştirinin sorumluluğundadır.
//string name = Console.ReadLine();
//var result = context.Database.ExecuteSqlRaw($"INSERT Persons VALUES('{name}')");
#endregion
#region SqlQuery
//SqlQuery fonksiyonu her ne kadar erişilebilir olsada artık desteklenememktedir. Bunun yerine DbSet propertysi üzerinden erişilebilen FromSql fonksiyonu gelmiştir/kullanılmaktadır.
#endregion
#region SqlQueryRaw
//SqlQueryRaw fonksiyonu her ne kadar erişilebilir olsada artık desteklenememktedir. Bunun yerine DbSet propertysi üzerinden erişilebilen FromSqlRaw fonksiyonu gelmiştir/kullanılmaktadır.
#endregion
#region GetMigrations
//Uygulamada üretilmiş olan tüm migration'ları runtime'da programatik olarak elde etmemizi sağlayan metottur.
//var migs = context.Database.GetMigrations();
//Console.WriteLine();
#endregion
#region GetAppliedMigrations
//Uygulamada migrate edilmiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur.
//var migs = context.Database.GetAppliedMigrations();
//Console.WriteLine();
#endregion
#region GetPendingMigrations
//Uygulamada migrate edilmemiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur.
//var migs = context.Database.GetPendingMigrations();
//Console.WriteLine();
#endregion
#region Migrate
//Migration'ları programatik olarak runtime'da migrate etmek için kullanılan bir fonksiyondur.
//context.Database.Migrate();
//EnsureCreated fonksiyonu migration'ları kapsamamaktadır. O yüzden migraton'lar içerisinde yapılan çalışmalar ilgili fonksiyonda geçerli olmayacaktır.

//Migratebekleyen tüm geçişleri veritabanına uygulayan, şemasını güncelleyen ve gerekirse verileri taşıyan daha güçlü bir yaklaşımdır. Veritabanının sık sık değişmesinin beklendiği ve uygulamanın veritabanı şemasını güncelleyebilmesi ve verileri güvenilir ve tutarlı bir şekilde taşıması gereken senaryolarda kullanılması amaçlanmıştır.
#endregion
#region OpenConnection
//Veritabanı bağlantısını manuel açar.
//context.Database.OpenConnection();
#endregion
#region CloseConnection
//Veritabanı bağlantısını manuel kapatır.
//context.Database.CloseConnection();
#endregion
#region GetConnectionString
//İlgili context nesnesinin o anda kullandığı connectionstring değeri ne ise onu elde etmenizi sağlar.
//Console.WriteLine(context.Database.GetConnectionString());
#endregion
#region GetDbConnection
//EF Core'un kullanmış olduğu Ado.NET altyapısının kullandığı DbConnection nesnesini elde etmemizi sağlayan bir fonksiyondur. Yaniiii bizleri Ado.NET kanadına götürür.

//SqlConnection connection = (SqlConnection)context.Database.GetDbConnection();
//Console.WriteLine();
#endregion
#region SetDbConnection
//Özelleştirilmiş connection nesnelerini EF Core mimarisine dahil etmemizi sağlayan bir fonksiyondur.
//context.Database.SetDbConnection();
#endregion
#region ProviderName Property'si
//saglayıcı adı nı  getırır orn sql orackel vb 
//EF Core'un kullanmış olduğu provider neyse onun bilgisini getiren bir proeprty'dir.
//Console.WriteLine(context.Database.ProviderName);
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost, 1433;Database=ApplicationDB;User ID=SA;Password=1q2w3e4r+!;TrustServerCertificate=True");
    }
}