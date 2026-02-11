using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class StatusTypeIdToPermitHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusTypeId",
                table: "PermitHistory",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PermitHistory_StatusTypeId",
                table: "PermitHistory",
                column: "StatusTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitHistory_StatusType_StatusTypeId",
                table: "PermitHistory",
                column: "StatusTypeId",
                principalTable: "StatusType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermitHistory_StatusType_StatusTypeId",
                table: "PermitHistory");

            migrationBuilder.DropIndex(
                name: "IX_PermitHistory_StatusTypeId",
                table: "PermitHistory");

            migrationBuilder.DropColumn(
                name: "StatusTypeId",
                table: "PermitHistory");
        }
    }
}
