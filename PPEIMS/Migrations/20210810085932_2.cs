using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionLiquidation",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TypeFuel",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Items",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionLiquidation",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeFuel",
                table: "Items",
                nullable: true);
        }
    }
}
