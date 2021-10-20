using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PPE",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "PPEId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_PPEId",
                table: "Items",
                column: "PPEId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_PPEs_PPEId",
                table: "Items",
                column: "PPEId",
                principalTable: "PPEs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_PPEs_PPEId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_PPEId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PPEId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "PPE",
                table: "Items",
                nullable: true);
        }
    }
}
