using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2._1.One_To_Many.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departmanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmanAdi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departmanlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "calisanler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calisanler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_calisanler_departmanlar_DepartmanId",
                        column: x => x.DepartmanId,
                        principalTable: "departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_calisanler_DepartmanId",
                table: "calisanler",
                column: "DepartmanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calisanler");

            migrationBuilder.DropTable(
                name: "departmanlar");
        }
    }
}
