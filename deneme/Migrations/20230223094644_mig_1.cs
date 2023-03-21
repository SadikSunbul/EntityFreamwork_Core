using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2._0.Bire_Bir.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calisanler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calisanler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "calisanAdresler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calisanAdresler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_calisanAdresler_calisanler_Id",
                        column: x => x.Id,
                        principalTable: "calisanler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calisanAdresler");

            migrationBuilder.DropTable(
                name: "calisanler");
        }
    }
}
