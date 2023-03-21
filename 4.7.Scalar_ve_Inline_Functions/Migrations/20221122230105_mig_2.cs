using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Functions.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                    CREATE FUNCTION getPersonTotalOrderPrice(@personId INT)
	                    RETURNS INT
                    AS
                    BEGIN
	                    DECLARE @totalPrice INT
	                    SELECT @totalPrice = SUM(o.Price) FROM Persons p
	                    JOIN Orders o
		                    ON p.PersonId = o.PersonId
	                    WHERE p.PersonId = @personId
	                    RETURN @totalPrice
                    END
                    ");
            /*
             Nasıl kullanılır fonksıyonlar
            select dbo.getPersonTotalOrderPrice(10)
            tek sartımız sema ıle cagırmak gerekır 
             */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP FUNCTION getPersonTotalOrderPrice");
        }
    }
}
