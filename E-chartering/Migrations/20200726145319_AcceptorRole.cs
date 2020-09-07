using Microsoft.EntityFrameworkCore.Migrations;

namespace Echartering.Migrations
{
    public partial class AcceptorRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptorRole",
                table: "ShipCargoRelations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptorRole",
                table: "ShipCargoRelations");
        }
    }
}
