// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
//cmd yazmadan once derlemek gerekır derlemezsen hata verıcektır
//dotnet ef migrations add [Migration Name] ------>dotnet ef migrations add mig_1

//migrations data baseye kaydetmek ıcın dotnet ef database update komutu kullanılır
//migrations geri alma da  dotnet ef database update [migrations name]

//BURASI ÇOOOOOOOKK ÖNEMLİ!!!!!!!!!!!!!!!!

EKayıtContext contex = new EKayıtContext();
#region Burada Kendımce bır gırıs kısımı olusturdum

string q = "E";
while (q=="E")
{
    Console.Clear();
    Console.WriteLine("Menü");
    Console.WriteLine("1->Kayıt ol");
    Console.WriteLine("2-> kayıt sil");
    Console.WriteLine("3-> giriş yap");
    Console.WriteLine("4-> kullanıcıları listele");
    Console.WriteLine("Bir rakaam seçiniz");
    string secın = Console.ReadLine();
    switch (secın)
    {
        case "1":
            Console.Clear();
            Console.WriteLine("Kayıt sayfasına hosgeldınız...");
            Kişi Kayıt = new ();
            Console.WriteLine("isim giriniz");
            Kayıt.İsim = Console.ReadLine();
            Console.WriteLine("soyisim giriniz");
            Kayıt.Soyisim = Console.ReadLine();
            Console.WriteLine("email girini");
            Kayıt.Email = Console.ReadLine();
            Console.WriteLine("şifrenizi giriniz");
            Kayıt.Şifre = Console.ReadLine();

           await contex.AddAsync(Kayıt);
            await contex.SaveChangesAsync();
            Console.WriteLine("kayıt işlemi başarılı");
            break;
        case "2":
            Console.WriteLine("kayıt silme sayfası");
            Console.WriteLine("silinecek kayıdın emailini giriniz");
            Kişi sil = new Kişi();
            sil.Email = Console.ReadLine();
            Console.WriteLine("silinecek kaydın şifresi");
            sil.Şifre = Console.ReadLine();

             Kişi silinecek=await contex.kişis.FirstOrDefaultAsync(i=>i.Email==sil.Email && i.Şifre==sil.Şifre);
            if (silinecek is null)
            {
                Console.WriteLine("kısı bulunamadı");
            }
            else
            {
                 contex.Remove(silinecek);
                await contex.SaveChangesAsync();
                Console.WriteLine("kısı sılındı");
            }
            break;
        case "3":
            Console.WriteLine("giriş sayfasına hoşgeldiniz");
            Console.WriteLine("email giriniz");
            string email = Console.ReadLine();
            Console.WriteLine("şifre giriniz");
            string şifre = Console.ReadLine();

            var giriş=await contex.kişis.AnyAsync(i => i.Email == email && i.Şifre == şifre);
            if (giriş)
            {
                Console.WriteLine("giriş basarılı");
            }
            else
            {
                Console.WriteLine("giriş başarısız");
            }
            break;
        case "4":
            var isimler=contex.kişis.Select(i =>i.İsim).ToList();
            foreach (var item in isimler)
            {
                Console.WriteLine(item);
            }
            break;
        default:
            break;
    }

    Console.WriteLine("Devam etmke istiyorsanız E yazınız istemiyorsanız H");
    q = Console.ReadLine().ToUpper();
}

#endregion

#region En Temel basit bir sorgulama nasıl yapılır?

#region Method Syntax --genelde burasını kullancaz
var kişiler=await contex.kişis.ToListAsync(); //kendısı sql sorgusu olusturup verılerı ceker buraya hepsını cek dedık
#endregion
#region Query Syntex
var kısıler2 =await (from kişi in contex.kişis
               select kişi).ToListAsync(); //burada kişiler içerisinden kişi hepsını cek dedık

#endregion

#endregion

#region Sorguyu Execute etmek için ne yapmamız gerekmektedir?
#region ToListAsync // IQureyable ToListAsync()--->execuit ederız burada --->IEbumerable doner

#region Method syntax
var kişiler = await contex.kişis.ToListAsync();
#endregion
#region Query Syntax
var kısıler2 = await (from kişi in contex.kişis
                      select kişi).ToListAsync();
#endregion

#endregion

int kissi_ıd = 4;

var kişiler = from Kişi in contex.kişis
              where Kişi.Id<kissi_ıd // && kişi.isim.contains(ali) ve isim de alı olanı getırcek 
              //sart verdık busarta uygun olanalrı getır dedık 
              select Kişi; //execuit edılmemıs   suanda IQueryable dayız

kissi_ıd = 200;  //burada yenı olusturulan sorguda 4 gıder 200 oalarak degısır sorgu usteydı nasıl degıstı bu
//execuit edılmeden dısarıdan alıcagı parametrelerı daha almadgı ıcın execuit edıldıgı zaman alıcaktır ondan dıakt edılmelı hata yapılmaamalı

foreach (var item in kişiler)
{
    Console.WriteLine(item.Soyisim);
}
#region Foreach

foreach (var kişi in kişiler) //burada cagırılınca kendısı execuit edıyor --->IEnumerable olur
{
    Console.WriteLine(kişi.İsim);
}

#region Deferred Execution(Ertelenmiş çalışma)
//IQueryable çalışmalarında ilgili kod yazıldıgı noktada tetıklenmez/calıstırılmaz yanı ilgiili kod yazıldıgı noktada
//sorguyu generate etmez ! nerede eder calıstırıldıgı/execuit edıldıgı noktada tetıklenır bu durumada Ertelenmiş çalışma denir

#endregion
#endregion
#endregion

#region IQueryable ve IEnumerable Nedir? Basit olarak

var kısıler = from Kişi in contex.kişis
              select Kişi; //sorgulaama asaması  IQureyable ToListAsync()--->execuit ederız burada --->IEbumerable doner

#region IQureyable
//sorguya karsılık gelir
//ef core uzerınde yapılmıs olan sorgunun execude edılmemıs halıni ifade eder
#endregion
#region IEnumerable
//sorgunun calıstırılıp/execute edilip verilerin in memorye yüklenmiş halini ifade eder
#endregion

#endregion

#region Çoğul veri getiren sorgulama fonksiyonları

#region ToLitAsync()
//üretilen sorguyu execute ettirmemizi sağlayan fonksiyondur.


#region Method syntax
var kişiler1 = await contex.kişis.ToListAsync();
#endregion

#region Query syntax
var kişiler2 = await(from kişi in contex.kişis
               select kişi).ToListAsync();
#endregion
#endregion


#region Where
//Olüşturulan sorguya where şartı eklememizi sağlayan bir fonlsiyondur

#region Methot syntax
var kısıler = await contex.kişis.Where(i => i.Id > 3).ToListAsync();
#endregion
#region Query syntax
var kisilier1 = from Kişi in contex.kişis
                where Kişi.Id > 3 && Kişi.Email.EndsWith("7")  //burada ıd sı 3 den buyuyk olanalr ve maıl ın sonu 7 olanalrı getır dedık
                select Kişi;
var data =await kisilier1.ToListAsync();
#endregion
#endregion

#region OrderBy
//sorgu uuzerınde sıralama yapmamamızı saglıyor  (Ascending)---->Yükselen
#region Method Syntax
var kisiler =await contex.kişis.Where(u => u.Id > 3 || u.İsim.EndsWith("a")).OrderBy(u=>u.Id).ToListAsync();
//burası ıd 3 den buyuk ve ısımın sonu a harfı olan order by ile ıd sı ne gore sıralama yaptırdık azdan coga 
#endregion

#region Query Syntax
var kişiler2 = await(from Kişi in contex.kişis
               where Kişi.Id>3 || Kişi.Email.StartsWith("A")  //burada bas harfı a olanalrı dedık veya
               orderby Kişi.Id //ıd ye gore sıralama ayapar kucukten buyuge dogru
               select Kişi).ToListAsync();
#endregion

#endregion

#region ThenBy
//order by uzerıne yapılan sıralama işlemini farklı kolonlarada uygulamamızı saglayan bir fonksıyondur. (Ascending)---->Yükselen
var kisiler3 = await contex.kişis.Where(u => u.Id > 3 || u.İsim.EndsWith("a")).OrderBy(u => u.Id).ThenBy(u => u.İsim).ToListAsync();
//burada ılk basta ıd ye gore bır sıralama yapar ıkıtane 3 varsa dıyelım bunlarıda kendı aralarında isime gore sırala demıs olduk bu daha fazla bır saekılde artada bılır

#endregion

#region OrderByDescending
//desending olarak sıralama yapmamızı saglayan bır fonksıyondur   (Dscending)---->Düşen
#region Method syntax
var kişiler5 = contex.kişis.OrderByDescending(i => i.Id);  //ıd ye gore bır sıralama yyaptık burda sart veermedık 
#endregion
#region Query syntax
var kişiler6 =await (from Kişi in contex.kişis
                orderby Kişi.Id
                select Kişi).ToListAsync();
#endregion
#endregion

#region ThenByDescending
//burdaki usteki şekilde
//OrderByDescending uzerıne yapılan sıralama işlemini farklı kolonlarada uygulamamızı saglayan bir fonksıyondur. (Dscending)---->Düşen

#endregion
#endregion


#region Tekil veri getiren sorgulama fonksıyonları

//yapılan sorguda sade tek verının gelmesı amaclanıyorsa single yada singleOrdefault fonksıyonları kullanıla bılır
#region SingleAsync
//Eğer ki, sorgu neticesinde birden fazla veri gelıyorsa yada hiç gelmiyorsa her iki durumdada exception fırlatır
#region Tek kayıt Geldiğinde
var kişi11 =await contex.kişis.SingleAsync(u=>u.Id==1); //ıd sı 1 olan deger oldugu ıcın hatasız bır sekılde calısacaktır
#endregion
#region Hiç kayıt gelmediğinde
var kişi12 = await contex.kişis.SingleAsync(u => u.Id == 55555); //boyle bır deger olmadıgından dolayyı hata verır
#endregion
#region Çok kayıt geldiğinde
var kişi13 = await contex.kişis.SingleAsync(u => u.Id >2); //ıd degerı 2 den buyuk olan bırden fazla deger var gene hata fırlatır
#endregion

#endregion


#region SingleOrDefaultAsync
//Eğerki sorgu neticesinde birden fazla veri geliyorsa exception fırlatır ,hiç veri gelmiyorsa null döner.
#region Tek kayıt Geldiğinde
var kişi14 = await contex.kişis.SingleOrDefaultAsync(u => u.Id == 1); //hata yok 

#endregion
#region Hiç kayıt gelmediğinde
var kişi15 = await contex.kişis.SingleOrDefaultAsync(u => u.Id == 5555); //null doner
#endregion
#region Çok kayıt geldiğinde
var kişi16 = await contex.kişis.SingleOrDefaultAsync(u => u.Id >1); //birden fazla verı getırdıgı ıcın hata fırlatıcaktır


#endregion
#endregion

#region FirstAsync
//Yapılan sorgudan Tek bır verının gelmesı amaclanıyorsa First Yada FirstOrDefault fonksıyonları Kullanıla bılır
//birden fazla ahmet olabılır ama bız 1 tamesını ıstıyoruz o durumlarda kullanılır
//Sorgu neticesinde elde edilen verilerden ilkini getirir.Eğerki hiç veri gelmiyorsa hata fırlatır
#region Tek Kayıt geldiğiinde
var kişiler11 = await contex.kişis.FirstAsync(x=>x.Id==2);
#endregion
#region Hiç kayıt gelmediğinde
var kişiler12 = await contex.kişis.FirstAsync(x => x.Id == 2222); //hata fırlatır

#endregion
#region Çok kayıt geldiğinde
var kişiler13 = await contex.kişis.FirstAsync(x => x.Id> 2); //burada 2 den sonra 3 ıd lı olanını bıze getırı

#endregion
#endregion

#region FirstOrDefaultAsync
//Sorgu netıcesınde elde edilen verilerden ilkini getirir egerkı hıc verı gelmıyorsa null değerini dondurur
#region Tek Kayıt geldiğiinde
var kişiler14 = await contex.kişis.FirstOrDefaultAsync(x => x.Id == 2);
#endregion
#region Hiç kayıt gelmediğinde
var kişiler15 = await contex.kişis.FirstOrDefaultAsync(x => x.Id == 2222); //null dondurur boyle bır kıullanıcı olmadıgı ıcın

#endregion
#region Çok kayıt geldiğinde
var kişiler16 = await contex.kişis.FirstAsync(x => x.Id > 2); //burada 2 den sonra 3 ıd lı olanını bıze getırı
//burada normalde 2 den buyuk ıd degerı olanları getırır burası ama bu sorguda ılk ını aldıgı ıcın 3 doner bıze
#endregion

#endregion

#region SingleAsync , SingleDefaultAsync, FirstAsync, FirstOrDeaultAsync fonksiyonlarının Karşılaştırması

#endregion

#region FindAsync

//Find fonksıyonu prymerıy key kolonuna ozel hılı bır sekılde sorgulama yapmamızı sagalar
Kişi kişi1=await contex.kişis.FindAsync(u=>u.Id==3);
Kişi kişi2 =await contex.kişis.FindAsync(3);//burada drekt olarak prymerı keye gore bır arama yapar
#region Commposite Primary Key Durumu

Kişi kişi3=await contex.kısıAdres.FirstAsync(2,1); //2. kişinin 1. adresi gıbı bır sorgu oldu burada
#endregion

#endregion

#region FindAsync ile SingleAsync , SingleOrDefaultAsync ,FirstAsync,FirstOrDefaultAsync fonksıyonlarının karşılatırması

#endregion

#region LastAsync
//orderby kullanılmalıdır
//burada en sondakını alır ustekını tersı
//sorgu netıcesınde gelen verilerden en sonuncusunu getırır hıc verı gelmıyorsa hata fırlatır
#endregion

#region LastOrdDefaultAsync
//orderby kullanılmalıdır
//burada en sondakını alır ustekını tersı  hata da null dondurur
#endregion

#endregion


#region Diğer sorgulama Fonksıyonları
#region CountAsync
//olusturulan sorgunun execute edılmesı netıcesınde kac adet satırın elde edileceği sayısal olarak (int) bızlere bıldıren fonktur
var kişiler20 =(await contex.kişis.ToListAsync()).Count();//burası kac kısının oldugunu gosterır ama malıyetlıdır burası
var kişiler21 =await contex.kişis.CountAsync(); //burada daha az bır malıyet yapmıs olduk uste ılk basta hepsını sorgulatıp sayıyorduk
                                           //burada drekt select count(*) form kişis yazmıs gıbı olduk
                                           //burad executeder bu fonk da 
#endregion
#region LongCountAsync
//aynı sekılde uste kı gıbı burada daha buyuk datalar ıcın kullanılır
var kişiler22 =await contex.kişis.LongCountAsync(u=>u.Id >2); //ustede aynı sekılde sart verebılırız
#endregion
#region AnyAsync
//sorgu netıcesınde verının gelıp gelmedıgını bool turunde donen fonkdur   "Varmı Yokmu"
var kişiler23 = await contex.kişis.Where(ı=>ı.Email.Contains("@")).AnyAsync(); //mailinde @ olanları ıkı tarafada sorgu yazıla bılır
var kişiler23 = await contex.kişis.AnyAsync(ı => ı.Email.Contains("@")); //ustekı ıle aynı sekilde calısır
#endregion
#region MaxAsnyc
var kişiler24 =await contex.kişis.MaxAsync(ı => ı.Id); //burada Id sı en buyuk olanı getırır
#endregion
#region MinAsync
var kişiler25 = await contex.kişis.MinAsync(ı => ı.Id); //burada Id sı en küçük olanı getırır

#endregion
#region Distinct 
//Sorguda tekrarlı kayıtlar varsa bunları tekıllestıren bır ısleve sahıp fonksıyondur.
var kişiler26 = await contex.kişis.Distinct().ToListAsync();
#endregion
#region AllAsync
//Bir sorgu netıcesındekı gelen verilerin verilen şarta uyup uymadığını kontrol etmektır. Eğerki tüm veriler şarta uyuyorsa true,uymuyorsa false döndürecektir
var m =await contex.kişis.AllAsync(i=>i.Id>2); //buarda kısılerın ıd degerlerı hepsının ıd sı 2 den buyulse true 1 tane bıle dusuk varsa yanı 1 ıd lı varsa false donucektır
#endregion
#region SumAsync
//Vermiş olduğumuz Sayısal propertynin toplamını alır
var ıdtoplam =await contex.kişis.SumAsync(u=>u.Id);
#endregion
#region AverageAsync
//vermiş olduğumuz sayısal propertynin aritmatik ortalamasını alır
var ıdortalama = await contex.kişis.AverageAsync(ı=>ı.Id);
#endregion
#region Contains
//Like '&...&' sorgusu olusturmaızı sagalr
var kişiler27 = await contex.kişis.ContainsAsync(); //burası o conteıs degil
//buradakı tanımdakı altakı contains()
var kişiler28 =  contex.kişis.Where(u => u.Email.Contains("a"));//mail icerisnde a harfı gecenlerı lıstelıycektır  
#endregion
#region StartsWith
//burada ...% olanları yapar
var kişiler29 = contex.kişis.Where(u => u.Email.StartsWith("a"));//maılde a harfı ıle baslıyanları getıtr bıze 
#endregion
#region EndssWith
//burada %... olanları yapar
var kişiler30 = contex.kişis.Where(u => u.Email.EndsWith("a")); //mailde a harfı ıle bitenleri getirir
#endregion

#endregion

#region Sorgu sonucu dönüşüm fonksiyonları
//bu fonksiyonlar ile sorgu neticesinde elde edilen verileri isteğimiz doğrultusunda farklı türlerde projecsiyon edebiliyoruz
#region ToDictionaryAsync
//Sorgu netıcesınde gelecek olan veriyi dictionary olarak elde etmek /tutmak /karşılamak ıstıyorsak kullanılır
var kişiler31 =await contex.kişis.ToDictionaryAsync(u=>u.Şifre,u=>u.Email);

//toList ile aynı amaca hizmet etmektedir yanı olusturulan sorguyu execude edıp neticesinde alırlar
//ToList:gelen sorgu neticesini entity turunde bır koleksıyonda(List<Entity>) donusturmekteyken
//ToDictionary: Gelen s orgu netıcesını Dictionery bır koleksıyona dondurecektır .---> Dictionery key ve value olarak kaydetmemize yarayan bir sınıfdır.
#endregion
#region ToArrayAsync
//Olusturulan sorguyu dizi olarak elde eder
//ToList ile muadil amaca hizmet eder yani sorguyu execude eder lakin gelen sonucu entity dizisi olarak elde eder.
var kişiler32 = await contex.kişis.ToArrayAsync();
#endregion
#region Select

//Select fonk işlevsel olarak bırden fazla  davranısı sozkonusudur ---->soradan execude etmek gerekır 
//1.Select fonk Generate edılecek sorgunun çekılecek kolonlarını ayarlamamızı saglamaktadır
var kişiler34 = contex.kişis.Select(i => new Kişi
{
    Id = i.Id,
    İsim = i.İsim
}).ToListAsync();//bu urunun sadece ıd ve isimi gelsın dedık sadece bunlar getırıldı bıze dıger columlar ısımıze yaramaz dedık sıstemı yormamıs olduk
//2. Select fonk Gelen verileri farklı türde karşılamamızı sağlar.T , anonım

var kişiler33 = contex.kişis.Select(i => new  //anonım calısıla bılır
{
    Id = i.Id,
    İsim = i.İsim
}).ToListAsync();

var kişiler35 = contex.kişis.Select(i => new denem //farklı bır ture de aktarıla bılır
{
    Id = i.Id,
    İsim = i.İsim
}).ToListAsync();




#endregion
#region SelectMany
//Selec ile aynı amaca hızmet eder lakin ilişkisel tablolar neticesinde gelen koleksıyonel verılerı de
//tekilleştirip projecsıyon etmemizi sağlar
var kişiler36 = contex.kişis.Include(i => i.adres).SelectMany(u => u.adres, (u, p) =>new
{
    u.Id,
    u.İsim,
    p.adres
}).ToList(); //inner join yapılandırması olusturur burası

#endregion
#endregion


#region Groupby Fonksiyonu
//Guruplama yapmamızı saglıyan fonktur
#region Methot Syntax
                    //id ye gore groupla dedık
var datas =await contex.kişis.GroupBy(i => i.Id).Select(group => new //bellı kolanları getırcez
{
    Count= group.Count(), //burada kaç urun geldıgını bulduk 
    id= group.Key //id degerıde group dakı key e karsılık gelır
}).ToListAsync();
#endregion



var datas1 =await (from Kişi in contex.kişis
             group Kişi by Kişi.Id
             into İsimverilirburaya
             select new
             {
                 id= İsimverilirburaya.Key,
                 count= İsimverilirburaya.Count()
             }).ToListAsync();

#region Query syntax


#endregion
#endregion

#region Foreach Fonksiyonu
//bır sorguala fonk felan degıldır
//sorgulama netcesinde elde edilen koleksiyonel veriler üzerinde iterasyonel olaral dönmemizi ve teker teker verileri
//elde edip işlemler yapabilmemizi sağlayan bir fonktur.Foerach döngüsünün methot halidir

foreach (var item in datas1)
{

}

datas1.ForEach(i =>
{
    Console.WriteLine(i.count);
    Console.WriteLine(i.id);
});
#endregion



class EKayıtContext :DbContext
{
    public DbSet<Kişi> kişis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Ekişi;User Id=sa ; Password=sıfre;TrustServerCertificate=true");
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
