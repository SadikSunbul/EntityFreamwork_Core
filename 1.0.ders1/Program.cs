// See https://aka.ms/new-console-template for more information

//Burası Database-Firt

//ilk önce server baglamak ıcın nuget paket ekleden sqlserver mıcrosoft olanı ekle bıde tools dan mıcrosofftu ekle

//cmd gir hangi projeye baglancaksa server cd [adresi] yazılır
//sonra oradan ındırmeler eksık ıse dotnet add package Microsoft.EntityFrameworkCore.SqlServer yazılabılır

//sonra kendı server bılgılerımızı gırmemız gerekir

//dotnet ef dbcontext scaffold "Server=localhost,1433;Initial
//Catalog=AdventureWorks2019;Persist Security Info=False;User
//ID=sa;Password=Sa05423653954;MultipleActiveResultSets=False;
//Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;
//" Microsoft.EntityFrameworkCore.SqlServer -o /Users/sadiksunbul/Projects/EntityFreamwork_Core_gençay_yıldız/1.0.ders1

//yazılır baglandıgı zaman dosyalar gelıcektır bıze ztn

//entity dedıgımız kısım lar  tablolarımızın orneklendıgı kısımlardır



using _1._0.ders1;

Product product = new();
product.Name = "Adjustable Race";



