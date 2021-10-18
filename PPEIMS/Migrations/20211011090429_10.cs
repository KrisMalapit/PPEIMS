using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "RequestDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "RequestDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "RequestDetails");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "RequestDetails");
        }
    }
}
