using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field",
                table: "DepartmentPPEs");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "DepartmentPPEs");

            migrationBuilder.AddColumn<int>(
                name: "Field",
                table: "PPEs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Office",
                table: "PPEs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field",
                table: "PPEs");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "PPEs");

            migrationBuilder.AddColumn<int>(
                name: "Field",
                table: "DepartmentPPEs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Office",
                table: "DepartmentPPEs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
