using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class updatePermitHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "PermitHistory",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNo",
                table: "PermitHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "PermitHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "PermitHistory",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermitHistory_DepartmentId",
            //    table: "PermitHistory",
            //    column: "DepartmentId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PermitHistory_Department_DepartmentId",
            //    table: "PermitHistory",
            //    column: "DepartmentId",
            //    principalTable: "Department",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermitHistory_Department_DepartmentId",
                table: "PermitHistory");

            migrationBuilder.DropIndex(
                name: "IX_PermitHistory_DepartmentId",
                table: "PermitHistory");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "PermitHistory");

            migrationBuilder.DropColumn(
                name: "EmployeeNo",
                table: "PermitHistory");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "PermitHistory");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "PermitHistory");
        }
    }
}
