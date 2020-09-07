using Microsoft.EntityFrameworkCore.Migrations;

namespace Echartering.Migrations
{
    public partial class doubleFreight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FixedFreight",
                table: "ShipCargoRelations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FreightIdea",
                table: "Cargos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FixedFreight",
                table: "ShipCargoRelations",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "FreightIdea",
                table: "Cargos",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
