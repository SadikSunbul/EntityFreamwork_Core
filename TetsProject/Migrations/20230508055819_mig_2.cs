using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TetsProject.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anneler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    İsim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anneler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Çocuklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    İsim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CocukId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Çocuklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Çocuklar_Anneler_CocukId",
                        column: x => x.CocukId,
                        principalTable: "Anneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Çocuklar_CocukId",
                table: "Çocuklar",
                column: "CocukId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Çocuklar");

            migrationBuilder.DropTable(
                name: "Anneler");
        }
    }
}
