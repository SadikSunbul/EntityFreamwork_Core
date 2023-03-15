// See https://aka.ms/new-console-template for more information

/*
 * Calısanlar 
 * +Id
 * +CalısanAdı
 * +DepartmanId
 * 
 * Departmanlar
 * +Id
 * +DepartmanAdı
 * 
 * Burada calısanlar departmanlara baglılar
 * burada tek basına var ola bılen tablo departmanlar tablosudur 
 * ama calısanlar departmanlar olmdan var olamaz ama departmanlar calısan olmadan var ola bılırler 
 */



#region Relationships (ilişkiler) Terimleri
#region Principal Entity(Asıl entitiy)
//Kendı basına var olabilen tobloyu modelleyen entitiye denır.
//departmanlar tablosunu modelleyen departman entitysidir
#endregion
#region Dependent Entity (Bağımlı Entitiy)
//Kendi basına var olamyan Bir başka tabloya bağımlı (İlişkisel olarak bagımlı) olan tabloyu modelleyen entitye denır

//Calısanlar tablosunu modelleyen calısan entıtysıdır
#endregion
#region Foreign Key
//Principal entıty  ile Dependent entıty arasındakı ılıskıyı saglayan key dır 
//yukarıdakı ornekte Foreign key DepartmanId dır 

//Dependent entıtyde tanımlanır  yanı bagımlı olanda tanımlanır 
//Principal entıtydekı prıncıpal keyı tutar
#endregion
#region Principal Key
//Principal entıtydekı ıd nın takendısıdır  Principal entıtynın kımlıgı olan kolonu ıfade eden propertydır
#endregion
class Calisan  //burada calısan ----> departmana baglı bır sekılde  --> kendı basına tabloyu modelleyemez
{
    public int Id{ get; set; }
    public string CalısanAdı { get; set; }
    public int DepartmanId { get; set; }

    //Navıgatıon ---->Property Olması gerekır 
    public Departman Departman { get; set; }  //bıre bir yanı calısanın sadece 1 tane departmanı oluyor burada 
    //burada tekıl bır sekılde yanı sadece bır departmanı alır   ---> Dependent tır burası --> tabloya baglı demek kenı dbasına tabloyu modelleye bılır 
}
class Departman   //burada departman ---> calısandan bagımsız bır selılde 
{
    public int Id { get; set; }
    public string DepartmanAdı { get; set; }

    //Navıgatıon ---->Property Olması gerekır 
    public ICollection<Calisan> CalisanLar { get; set; }  //bire cok yanı bır departman bırden fazala calısanın olabılır 
    //burada collectıon oldugundan dolayı cogul ılıskısı vardır burada --->Principal dır burası  --> tek basına tablo olustura bılır burası 
}
#endregion

#region ******* Navigation Property Nedir? ********
//İlişkısel tablolar arasındakı fısıksel erısımı entıty class ları uzerınden saglayan propertylerdır

//bir propertynın navıgatıon property oolması ıcın kesınlıkle entıty turunden olması gerekıyor

//buranın ornegını bı uste yaptık 

//Navıgatıon propertyler Entıtylerdekıtanımların  gore n e n veyahut 1 e n seklınde ılıskı turlerını ıfade etmektedırler sonrakı derslerımızde ilişkısel verı yapılarını tam tefarrautlı pratıkte ıncelerken navıgatıon propertylerın bu ozellıklerınden ıstıfade ettgımızı goreceksınız 


//Burada bahsettıgımız navıgatıon property 
//public Departman Departman { get; set; }  lerdır veya coklu ılıskılerde 
//public ICollection<Calisan> CalisanLar { get; set; } 
#endregion

#region İlişki Türeleri
//Teferautlar ılerıde detaylanacak 
#region One to One
//Çalışan ile adresı arsındakı ılıskı her adreste bır kısı oturdugunu dusunelım ve herbır calısanın adresı farklı oldugundan 1 e 1 ılıskı olmus olur yanı bır adres baska bır calısana gıtmedıgınden dolayı 1 e 1 dır 
//karı koca arasındakı ılıskı  ----->  heer bır kocanın sadece bır tane karısı olucagından dolayı bu ornekte 1 e 1 e ornektır 
#endregion
#region One to Many
//Bır e Cok ılıskı 
//Anne cocuk ılıskı 
//her bır annenın bırden fazla cocugu olabılır ama her bır cocugun sadece bır tane annesı olucagından bu ornek bıre cok bır ornektır 
#endregion
#region Many to Many
//coka cok
//calısanalr ıle projeler arasındakı ılıskı 
//kardesler arasındakı ılıskı
#endregion
#endregion

#region EntiyFreamework Core'da İlişki Yapılandırma Yöntemleri
#region Default Conventions
//Varsayılan entty kurallarını kullanarak yapılan ilişki yapılandırma yontemlerıdır

//Navıgatıon propertylerı kullanarak ılıskı şabonlarını cıkartmaktadır
#endregion
#region Data Annotaions Attributes
//entıtynın nıtelılerıne gore ınce ayarlarımızı yapmamızı sagalayan Attribute lardır [Key],[ForeignKey]
#endregion
#region Fluent 
//Entıty modellerındekı ılıskılerı yapılandırırken daha detaylı calısmamızı saglayan yontemdır

#region HasOne
//bası 1 ıle baslıyanar 
//ılgılı entıtynın ılıskısel entıtye bırebır veya bıre cok olacak sekılde ılıskısını yapılandırmaua baslayan metottur
#endregion
#region HasMany
//Bası cok ıle baslıyanlsr
//ılgılı entıtynın ılıskısel entıtye coka bır veya coka cok olacak sekılde ılıskısını yapılandırmaua baslayan metottur
#endregion
#region WithMany
//sonu 1 le bıtenler  ---> 1 e 1 ---> cok  a 1
//HasOne ya da HasMany den sonra bıre bır yada coka bir sekılde ılıskı yapılandırmsını tamamlayan metotur
#endregion
#region WhiteMany
//sonu cokla bıtenler ---> 1 e cok  ---> cok a cok
//HasOne ya da HasMany den sonra bıre cok  yada coka cok sekılde ılıskı yapılandırmsını tamamlayan metotur

#endregion
#endregion
#endregion

