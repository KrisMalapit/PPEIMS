using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_PPEs_PPEId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "PPEId",
                table: "Items",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Items_PPEs_PPEId",
                table: "Items",
                column: "PPEId",
                principalTable: "PPEs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_PPEs_PPEId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "PPEId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_PPEs_PPEId",
                table: "Items",
                column: "PPEId",
                principalTable: "PPEs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
