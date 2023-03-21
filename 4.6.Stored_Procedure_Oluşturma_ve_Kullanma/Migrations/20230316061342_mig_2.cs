using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._6.Stored_Procedure_Oluşturma_ve_Kullanma.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                Create Procedure sp_PersonOrders
             As   
                select * from Person p
                joinn Orders o
                    On p.PersonId=o.PersonId
                Group By p.Name
                Order By Count(*) Desc

            ");

            /*
             burada bı tane ornek yapıcaz deger alan ıcerısıne seklınde 
            --output ıcerıdekı degerı dısarıya gonderır
            Create Proc Urungetir
            (
                @Fiyat Money   
             )As
            Select * from Urunler Where BirimFiyat > @Fiyat
            return @@rowcount   --buradakı uste kac satır etkılendıyse onun sayısnı dondurur

            --Değişken tanımlama --_>DECLARE @isim veritipi
            --Kullanımı 
            Exec Urungetir 40    -- gerı donen degerı elde edemeyız burada
            Declare @Sonuc int
            Exec @sonuc=Exec Urungetir 40
           
             */


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                Drop Procedure sp_PersonOrders
             As   
                select * from Person p
                joinn Orders o
                    On p.PersonId=o.PersonId
                Group By p.Name
                Order By Count(*) Desc

            ");
        }
    }
}
