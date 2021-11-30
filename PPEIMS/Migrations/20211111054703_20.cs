using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPEIMS.Migrations
{
    public partial class _20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartmentApprovedDate",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DepartmentHeadId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SafetyApprovedDate",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SafetyId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "WarehouseApprovedDate",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WarehousemanId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DepartmentApprovedDate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DepartmentHeadId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SafetyApprovedDate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SafetyId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WarehouseApprovedDate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WarehousemanId",
                table: "Requests");
        }
    }
}
