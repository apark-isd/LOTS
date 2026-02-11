using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class RequestPickDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PermitPickDate",
                table: "Permittee",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PermitRequestDate",
                table: "Permittee",
                nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "StatusTypeId",
            //    table: "Permittee",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StatusTypeId",
                table: "Permit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Permittee_StatusTypeId",
                table: "Permittee",
                column: "StatusTypeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Permittee_StatusType_StatusTypeId",
            //    table: "Permittee",
            //    column: "StatusTypeId",
            //    principalTable: "StatusType",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permittee_StatusType_StatusTypeId",
                table: "Permittee");

            migrationBuilder.DropIndex(
                name: "IX_Permittee_StatusTypeId",
                table: "Permittee");

            migrationBuilder.DropColumn(
                name: "PermitPickDate",
                table: "Permittee");

            migrationBuilder.DropColumn(
                name: "PermitRequestDate",
                table: "Permittee");

            migrationBuilder.DropColumn(
                name: "StatusTypeId",
                table: "Permittee");

            migrationBuilder.AlterColumn<int>(
                name: "StatusTypeId",
                table: "Permit",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
