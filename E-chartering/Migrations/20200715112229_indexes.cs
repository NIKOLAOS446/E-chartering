using Microsoft.EntityFrameworkCore.Migrations;

namespace Echartering.Migrations
{
    public partial class indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ships",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Cargos",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ships_UserId",
                table: "Ships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_UserId",
                table: "Cargos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cargos_AspNetUsers_UserId",
                table: "Cargos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_AspNetUsers_UserId",
                table: "Ships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cargos_AspNetUsers_UserId",
                table: "Cargos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ships_AspNetUsers_UserId",
                table: "Ships");

            migrationBuilder.DropIndex(
                name: "IX_Ships_UserId",
                table: "Ships");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_UserId",
                table: "Cargos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ships",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Cargos",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
