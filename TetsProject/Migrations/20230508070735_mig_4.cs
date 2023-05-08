using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TetsProject.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Çocuklar_Anneler_CocukId",
                table: "Çocuklar");

            migrationBuilder.RenameColumn(
                name: "CocukId",
                table: "Çocuklar",
                newName: "AnneId");

            migrationBuilder.RenameIndex(
                name: "IX_Çocuklar_CocukId",
                table: "Çocuklar",
                newName: "IX_Çocuklar_AnneId");

            migrationBuilder.AlterColumn<string>(
                name: "İsim",
                table: "Çocuklar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "İsim",
                table: "Anneler",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Çocuklar_Anneler_AnneId",
                table: "Çocuklar",
                column: "AnneId",
                principalTable: "Anneler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Çocuklar_Anneler_AnneId",
                table: "Çocuklar");

            migrationBuilder.RenameColumn(
                name: "AnneId",
                table: "Çocuklar",
                newName: "CocukId");

            migrationBuilder.RenameIndex(
                name: "IX_Çocuklar_AnneId",
                table: "Çocuklar",
                newName: "IX_Çocuklar_CocukId");

            migrationBuilder.AlterColumn<string>(
                name: "İsim",
                table: "Çocuklar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "İsim",
                table: "Anneler",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Çocuklar_Anneler_CocukId",
                table: "Çocuklar",
                column: "CocukId",
                principalTable: "Anneler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
