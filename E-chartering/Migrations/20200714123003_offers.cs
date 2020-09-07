
using Microsoft.EntityFrameworkCore.Migrations;

namespace Echartering.Migrations
{
    public partial class offers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "CargoUserId",
                table: "ShipCargoRelations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ShipCargoRelations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShipUserId",
                table: "ShipCargoRelations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Cargos",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CargoUserId",
                table: "ShipCargoRelations");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ShipCargoRelations");

            migrationBuilder.DropColumn(
                name: "ShipUserId",
                table: "ShipCargoRelations");

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
    }
}
