using Microsoft.EntityFrameworkCore.Migrations;

namespace Echartering.Migrations
{
    public partial class userindexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShipUserId",
                table: "ShipCargoRelations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CargoUserId",
                table: "ShipCargoRelations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipCargoRelations_CargoUserId",
                table: "ShipCargoRelations",
                column: "CargoUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipCargoRelations_ShipUserId",
                table: "ShipCargoRelations",
                column: "ShipUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipCargoRelations_AspNetUsers_CargoUserId",
                table: "ShipCargoRelations",
                column: "CargoUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShipCargoRelations_AspNetUsers_ShipUserId",
                table: "ShipCargoRelations",
                column: "ShipUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipCargoRelations_AspNetUsers_CargoUserId",
                table: "ShipCargoRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipCargoRelations_AspNetUsers_ShipUserId",
                table: "ShipCargoRelations");

            migrationBuilder.DropIndex(
                name: "IX_ShipCargoRelations_CargoUserId",
                table: "ShipCargoRelations");

            migrationBuilder.DropIndex(
                name: "IX_ShipCargoRelations_ShipUserId",
                table: "ShipCargoRelations");

            migrationBuilder.AlterColumn<string>(
                name: "ShipUserId",
                table: "ShipCargoRelations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CargoUserId",
                table: "ShipCargoRelations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
