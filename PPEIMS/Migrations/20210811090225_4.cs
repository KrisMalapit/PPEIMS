using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LineNo",
                table: "ItemDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ItemDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineNo",
                table: "ItemDetails");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ItemDetails");
        }
    }
}
