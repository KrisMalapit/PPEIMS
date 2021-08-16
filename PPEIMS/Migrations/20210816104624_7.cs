using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDetails_Users_UserId",
                table: "RequestDetails");

            migrationBuilder.DropIndex(
                name: "IX_RequestDetails_UserId",
                table: "RequestDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RequestDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RequestDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RequestDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RequestDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RequestDetails");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RequestDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetails_UserId",
                table: "RequestDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDetails_Users_UserId",
                table: "RequestDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
