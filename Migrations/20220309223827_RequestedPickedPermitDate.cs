using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class RequestedPickedPermitDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermitPickDate",
                table: "Permittee");

            migrationBuilder.DropColumn(
                name: "PermitRequestDate",
                table: "Permittee");

            migrationBuilder.AddColumn<DateTime>(
                name: "PickedPermitDate",
                table: "Permittee",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedPermitDate",
                table: "Permittee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickedPermitDate",
                table: "Permittee");

            migrationBuilder.DropColumn(
                name: "RequestedPermitDate",
                table: "Permittee");

            migrationBuilder.AddColumn<DateTime>(
                name: "PermitPickDate",
                table: "Permittee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PermitRequestDate",
                table: "Permittee",
                type: "datetime2",
                nullable: true);
        }
    }
}
