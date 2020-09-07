using Microsoft.EntityFrameworkCore.Migrations;

namespace Echartering.Migrations
{
    public partial class fixedfreight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FixedFreight",
                table: "ShipCargoRelations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedFreight",
                table: "ShipCargoRelations");
        }
    }
}
