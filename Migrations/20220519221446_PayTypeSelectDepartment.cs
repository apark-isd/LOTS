using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class PayTypeSelectDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "PayType",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PayType_DepartmentId",
                table: "PayType",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayType_Department_DepartmentId",
                table: "PayType",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayType_Department_DepartmentId",
                table: "PayType");

            migrationBuilder.DropIndex(
                name: "IX_PayType_DepartmentId",
                table: "PayType");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "PayType");
        }
    }
}
