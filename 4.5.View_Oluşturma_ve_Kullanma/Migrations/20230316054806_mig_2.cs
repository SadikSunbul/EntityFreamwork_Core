using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._5.View_Oluşturma_ve_Kullanma.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            //View olusturma MICRATE EDILIRKEN OLUSUR 

            migrationBuilder.Sql(@"
                CREATE VIEW vm_PersonOrders
               As
                    Select TOP 1OO p.Name,Count(*) [Count] FROM Person p
                    INNER JOIN Orders o
                        Onp.PersonId=o.PersonId
                   GROUP By p.Name
                   Order By [Count] DESC
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //BURDA SILMIS OLDUK MIGRATE GERI ALINIRSA 
            migrationBuilder.Sql(@"
                DROP VIEW vm_PersonOrders
               As
                    Select TOP 1OO p.Name,Count(*) [Count] FROM Person p
                    INNER JOIN Orders o
                        Onp.PersonId=o.PersonId
                   GROUP By p.Name
                   Order By [Count] DESC
            ");

        }
    }
}
