// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;



//code-first
//cmd yazmadan once derlemek gerekır derlemezsen hata verıcektır
//dotnet ef migrations add [Migration Name] ------>dotnet ef migrations add mig_1

//migrations data baseye kaydetmek ıcın dotnet ef database update komutu kullanılır
//migrations geri alma da  dotnet ef database update [migrations name]


//kod uzerınden migrate etmek


EKayıtContext context = new EKayıtContext();


#region Cahnge Treking Nedir?
//context nesnesi üzerinden gelen tüm nesneler/veriler otomatik olarak birtakip mekanizması tarafından
//izlenirler işte bu takip mekanizmasına Change Tracker denir

//Change tracker ıle nesneler üzerindekideğişiklişkler/işlemler takip edilerek netice itibariyle bu
//işlemlerin fıtratına uygun sql sorgucukları generete edilir.İşte bu işlemede Change Tracking denir.


#endregion

#region CahngeTracker Propertysi
//takip edilen nesnelere erişebilmemizi sağlayan ve gerektiği takdırde işlemler gerçekleştirmemizi sağlayan bir propertydir
//Contex sınıfının base clası olan DbContext sınıfının bir member'ıdır

var kısıler = await context.kişis.ToListAsync(); //burada gelecek verıler takıbe alınacak otomatık olarak 
kısıler[2].Soyisim = "sakşfljk";//update
context.kişis.Remove(kısıler[3]);//delete

var datas= context.ChangeTracker.Entries();//burada butuntakıp edılen nesnelerı gosterır bıze
//bızım yaptıgımız degısıklıklerı algılar ve tutar kaydet demezsek salar bunları
//kaydetmeyı--->await context.SaveChangesAsync();---> seklınde yaparız

Console.ReadLine();
#endregion
#region DetectChanges Metodu  -->Degısıklıklerı Anlgıla
//EF core context nesnesi tarafından izlenen tüm nesnelerin değişiklikleri Change Taracker sayesınde takıp edebilmekte
//ve nesnelerde olan verisel değişiklikler yakalanarak bunların anlık görüntüleri (snapshot) ini oluşturabilir

//Yapılan değişiklikleri veritabanına gönderilmeden önce algılandığından emin olmak gerekir.SaveChanges fonk
//cagrıldıgı andan nesneler EF Core tarafından otomatik kontrol edilirler.

//Ancak yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişikliklerin algılanmasını
//opsiyonel olarak gerçekleştirmek istiyebiliriz .İşte bunun için DetectChanges fonksiyonu kullanılabilir ve
//her ne kadar EF Core değişikleri otomatik algılıyor olsada siz yine de iradenizle kontrole zorlıya bilirsiniz

var kişiler1 = await context.kişis.FirstOrDefaultAsync(u=>u.Id==2);
kişiler1.Id = 5;
context.ChangeTracker.DetectChanges(); //burayı yazmaya gerek yok aslında kendısı algılar algılamadıgı durumunda yazılabılır
//Değişiklikleri ChangeTracker kullanarak algıladıktan sonra, değişiklikler SaveChangesAsync yöntemi kullanılarak veritabanına kaydedilir.
await context.SaveChangesAsync(); //kendısı ustekını cagırıyor sen ıstersen garantıye al yaz ustekını
// context.ChangeTracker.DetectChanges();  bu fonk pek dokunmayız bız zaten kendısı yapıyor 
#endregion
#region AutoDetectChangesEnabled Property'si
//İlkgili metotlarda (Savechanges , Entries) tarafından DetectChanges metodunun otomatık olarak tetıklenmesini
//configürasyonunu yapmamızı saglayan propertydır.

//SaveChanges fonksıyonu tetıklendıgınde DetectChanges metodunu içerisinde deffault olarak çagırmaktadır.Bu durumda
//DetectChanges fonksıyonu kullanımı irademizle yönetmek ve maliyet/performans optımızasyonu yapmak istediğimiz
//durumlarda AutoDetectChangesEnable özelliğini kapata bilirirz

#endregion
#region Entries Metodu
//Contexte ki entry metodunun kkoleksiyonel versiyonudur
//change tracker mekanizması tarafından izlenen her entity nesnesinin bilgisini EntityEntry elde etmemizi sağlar
//ve belirli işlemler yapabilmemize olanak tanır.

//Entries metodu, DetectChanges metodunu tetikler --> sonkez bır kontrol etırır en guncelı almak ıstedıgı ıcın 
//Bu durumda tıpkı SaveChanges'da oldugu gibi bir malıyettır.
//buradakı malıyetten kacınmak ıcın AutoDetectChangesEnabled ozellıgıne false degerı verıle bılrı

var kişiler3 = await context.kişis.ToListAsync();
kişiler3.FirstOrDefault(i=>i.Id==2).Id = 5;
context.kişis.Remove(kişiler3.FirstOrDefault(i => i.Id == 3));

context.ChangeTracker.Entries().ToList().ForEach(i =>
{
    if (i.State==EntityState.Unchanged)
    {
        //bısey apılmadıysa burayı uygula
    }
    else if (i.State==EntityState.Deleted)
    {
        //silme yaptıysan burayı tetıkle dedık
    }
});

#endregion
#region AcceptAllChanges Methodu
//SaveChanges veya SaveChanges(true) olarak tetıklendıgı zaman EF Core herseyın yolunda oldugunu varsayarak
//track ettıgı verılerın takıbını keser yenı degısıklıklerın takıp edılmesını bekler .Boyle bır durumda beklenmeyen
//bır durum/olası bır hata soz konusu olursa eger EF Core takıp etıgı nesnelerı brakacagı ıcın bır duzeltme mevzu bahsıs
//olamayacaktır

//Halıyle bu durumda devreye SaveChanges(false) ve AcceptAllChanges metotları gırecektır
//AcceptAllChanges --> bu ıradelı bır sekılde verılerın takıbının brakılmasını saglıyor SaveChanges(false) burda
//verıelr hatasız kaydedıldıyse onu sonuna kullanırız

// SaveChanges(false), EF Core gereklı verıtabanı komutlarını yurutmesını soyler ancak gerektıgınde yenıden oynatabılmesı
//için değişikleri beklemeye/nesnelerı takıp etmeye devam eder Taa ki AcceptAllChanges metodu irademizle cagırana kadar!

//SaveChanges(false) ile işlemin başarılı olduğuna emin olursanız AcceptAllChanges metodu ile nesne takibini
//kesebilirsiniz

var kişiler4 = await context.kişis.ToListAsync();
kişiler3.FirstOrDefault(i => i.Id == 2).Id = 5;
context.kişis.Remove(kişiler3.FirstOrDefault(i => i.Id == 3));
await context.SaveChangesAsync(); ----> await context.SaveChangesAsync(true); //ıkısıde aynı ısı gorur -->Basarılı veya basarısız CahngeTracker tutugu seylerı brakır
await context.SaveChangesAsync(false); // burada CahngeTracker brakmaz elındekı verılerı hata oldugunda duzelte bılırız
context.ChangeTracker.AcceptAllChanges(); //burası takıbı keser
#endregion
#region HasChanges Methodu
//Takip edilen nesneler arasında değişiklik yapılanların olup olmadıgını kontrol eder
//Arka planda DetectChanges metodunu tetıkler
context.ChangeTracker.HasChanges(); //bool tur doner
#endregion
#region Entity States
//Entity nesnelerın durumlarını ıfade eder

#region Deteched
//nesnenın change tracker tarafından takip edılmedıgını ifade eder
Kişi kişsi = new();  //-->contexten gelmedıgı ıcın takıp edılmez
Console.WriteLine(context.Entry(kişsi).State);
#endregion

#region Added
//verı tabanına eklenecek nesneyı ıfade eder
//henuz verı tabanına  işlenmemiş veriyi ifade eder SaveChanges fonksıyonu cogrıldıgında insert sorgusu
//oluşturulacağı anlamına gelır

Kişi kişş = new() {   Email="knşjlhghfhf"};
Console.WriteLine(context.Entry(kişş).State); //takıp edılmıyor der ılk -->Deteched
await context.AddAsync(kişsi); //bu saaten sonra Track trager takıp etmeye baslaa
Console.WriteLine(context.Entry(kişş).State); //added
await context.SaveChangesAsync(); 
Console.WriteLine(context.Entry(kişş).State);
#endregion

#region Unchanged
//verı tabanından sorgulandıgındanberı nesne uzerınde herhangı bır degısıklık yapılmadıgını ifade eder
// Sorgu netcesinde elde edılen tüm nesneler başlangıçta bu state değerindedir

var kısıssad = await context.kişis.ToListAsync ();
var data = context.ChangeTracker.Entries(); //bsıey yapılmadı der
Console.ReadLine();
#endregion

#region Modified
//nesne uzerınde degısıklık yanı guncelleme yapıldıgını gosterır
//SaveChanges fonk cagrıldıgında update sorgusunun olusturuşacagı anlamına gelır

#endregion

#region Deleted
//Nesnenın sılındıgını ıfade eder 
//SaveChanges fonk cagrıldıgında deleted fonk calısaganı ıfade eder

#endregion

#endregion
#region Context Nesnesi üzerinden Change Trackersadık

var kişisadi = await context.kişis.FirstOrDefaultAsync(i=>i.Id==2);
kişisadi.Soyisim = "Taha";
kişisadi.Email = "asfasf"; //update

#region Entry Methodu
#region Orginal Values Property'si
var Email=context.Entry(kişisadi).OriginalValues.GetValue<string>(nameof(kişisadi.Email));
//yukarıda degıstırdıgımız seyı degısmemıs gıbı getırcek bıze
//Verı tabnına kaydedılmemıs yukarıdakınler o yuzden bu kullanıla bılır ama verı tabanına kaydedılmıs olsaydı bunlara ulasamazdırk

#endregion
#region CurrentValues Property'si
//suankı mevcut degerinı getırı verı tabanındakı degıl 
var kısıadı=context.Entry(kişisadi).CurrentValues.GetValue<string>(nameof(kişisadi.İsim));
#endregion
#region GetDatabaseValues Methodu
//
var _kişi=await context.Entry(kişisadi).GetDatabaseValuesAsync();

#endregion
#endregion
#endregion
#region Change Tracker'in Interceptor Olarak Kullanılması
// 


#endregion


Console.ReadLine();

class EKayıtContext : DbContext
{
    public DbSet<Kişi> kişis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Ekişi;User Id=sa ; Password=sıfre;TrustServerCertificate=true");
    }
    //savechanges overıde ettık burada
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries();
        foreach (var item in entries)
        {
            if (item.State==EntityState.Added)
            {
                //burada kayıt ıslemınden once araya gırıp farklı ıslemler yaptıra bılırız
                //genelde heryerde etıcarette vb seyler
                //
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}


class Kişi
{
    public int Id { get; set; }
    public string İsim { get; set; }
    public string Soyisim { get; set; }
    public string Email { get; set; }
    public string Şifre { get; set; }
    public ICollection<Adres> adres { get; set; } //burada ilşkisel yapmı olduk

}

public class Adres
{
    public int Id { get; set; }
    public string adres { get; set; }
}

class denem
{
    public int Id { get; set; }
    public string İsim { get; set; }
}
