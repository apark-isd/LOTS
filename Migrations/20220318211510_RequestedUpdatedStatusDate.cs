using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class RequestedUpdatedStatusDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickedPermitDate",
                table: "Permittee");

            migrationBuilder.DropColumn(
                name: "RequestedPermitDate",
                table: "Permittee");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDate",
                table: "Permittee",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedStatusDate",
                table: "Permittee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestedDate",
                table: "Permittee");

            migrationBuilder.DropColumn(
                name: "UpdatedStatusDate",
                table: "Permittee");

            migrationBuilder.AddColumn<DateTime>(
                name: "PickedPermitDate",
                table: "Permittee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedPermitDate",
                table: "Permittee",
                type: "datetime2",
                nullable: true);
        }
    }
}
