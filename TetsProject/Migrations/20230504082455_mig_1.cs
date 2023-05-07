using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TetsProject.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "xSequence");

            migrationBuilder.CreateTable(
                name: "q",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [xSequence]"),
                    Xaçıklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qaçıkalam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_q", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "x",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [xSequence]"),
                    Xaçıklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_x", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "y",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [xSequence]"),
                    Xaçıklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yaçıklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_y", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "z",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [xSequence]"),
                    Xaçıklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yaçıklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zaçıklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "q");

            migrationBuilder.DropTable(
                name: "x");

            migrationBuilder.DropTable(
                name: "y");

            migrationBuilder.DropTable(
                name: "z");

            migrationBuilder.DropSequence(
                name: "xSequence");
        }
    }
}
