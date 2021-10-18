using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Field",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Office",
                table: "Items",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "Items");
        }
    }
}
