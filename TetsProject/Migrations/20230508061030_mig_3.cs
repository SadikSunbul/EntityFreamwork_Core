using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TetsProject.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ec_sec",
                startValue: 990L);

            migrationBuilder.AlterColumn<string>(
                name: "İsim",
                table: "Anneler",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Rastgelesayi",
                table: "Anneler",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR ec_sec");

            migrationBuilder.CreateIndex(
                name: "IX_Anneler_İsim",
                table: "Anneler",
                column: "İsim");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Anneler_İsim",
                table: "Anneler");

            migrationBuilder.DropColumn(
                name: "Rastgelesayi",
                table: "Anneler");

            migrationBuilder.DropSequence(
                name: "ec_sec");

            migrationBuilder.AlterColumn<string>(
                name: "İsim",
                table: "Anneler",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
