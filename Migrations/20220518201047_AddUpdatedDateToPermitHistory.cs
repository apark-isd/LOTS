using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class AddUpdatedDateToPermitHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PermitHistory",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermitHistory_StatusTypeId1",
            //    table: "PermitHistory",
            //    column: "StatusTypeId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PermitHistory_StatusType1_StatusTypeId1",
            //    table: "PermitHistory",
            //    column: "StatusTypeId1",
            //    principalTable: "StatusType1",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PermitHistory_StatusType1_StatusTypeId1",
            //    table: "PermitHistory");

            //migrationBuilder.DropIndex(
            //    name: "IX_PermitHistory_StatusTypeId1",
            //    table: "PermitHistory");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PermitHistory");
        }
    }
}
