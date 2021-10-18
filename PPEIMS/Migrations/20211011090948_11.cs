using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "RequestDetails",
                newName: "Remarks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "RequestDetails",
                newName: "Reason");
        }
    }
}
