using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class AddStatusToCommissioner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<DateTime>(
            //    name: "DateInput",
            //    table: "Vacancy",
            //    nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "StatusTypeId1",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "StatusTypeId",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "PermitteeTypeId",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "PermitteeId",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "PermitTypeId",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "PayTypeId",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "LotId",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "DepartmentId",
            //    table: "PermitHistory",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StatusTypeId",
                table: "Commissioner",
                nullable: true,
                defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Commissioner_StatusTypeId",
            //    table: "Commissioner",
            //    column: "StatusTypeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Commissioner_StatusType_StatusTypeId",
            //    table: "Commissioner",
            //    column: "StatusTypeId",
            //    principalTable: "StatusType",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Commissioner_StatusType_StatusTypeId",
            //    table: "Commissioner");

            //migrationBuilder.DropIndex(
            //    name: "IX_Commissioner_StatusTypeId",
            //    table: "Commissioner");

            //migrationBuilder.DropColumn(
            //    name: "DateInput",
            //    table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "StatusTypeId",
                table: "Commissioner");

            //migrationBuilder.AlterColumn<int>(
            //    name: "StatusTypeId1",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "StatusTypeId",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "PermitteeTypeId",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "PermitteeId",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "PermitTypeId",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "PayTypeId",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "LotId",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "DepartmentId",
            //    table: "PermitHistory",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);
        }
    }
}
