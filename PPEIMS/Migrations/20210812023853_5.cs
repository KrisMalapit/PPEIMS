using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DispenserAccess",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LubeAccess",
                table: "Users",
                newName: "Category");

            migrationBuilder.AddColumn<string>(
                name: "PPE",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PPE",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Users",
                newName: "LubeAccess");

            migrationBuilder.AddColumn<string>(
                name: "DispenserAccess",
                table: "Users",
                nullable: true);
        }
    }
}
