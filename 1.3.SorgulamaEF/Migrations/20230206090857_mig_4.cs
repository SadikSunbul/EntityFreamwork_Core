using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1._3.SorgulamaEF.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KişiId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adres_kişis_KişiId",
                        column: x => x.KişiId,
                        principalTable: "kişis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adres_KişiId",
                table: "Adres",
                column: "KişiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adres");
        }
    }
}
