using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class DepartmentIdToPermit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Permit",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Permit_DepartmentId",
                table: "Permit",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permit_Department_DepartmentId",
                table: "Permit",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permit_Department_DepartmentId",
                table: "Permit");

            migrationBuilder.DropIndex(
                name: "IX_Permit_DepartmentId",
                table: "Permit");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Permit");
        }
    }
}
