using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class RemovePayTypeDeptId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PayType_Department_DepartmentId",
            //    table: "PayType");

            //migrationBuilder.DropIndex(
            //    name: "IX_PayType_DepartmentId",
            //    table: "PayType");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "PayType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "PayType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PayType_DepartmentId",
            //    table: "PayType",
            //    column: "DepartmentId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PayType_Department_DepartmentId",
            //    table: "PayType",
            //    column: "DepartmentId",
            //    principalTable: "Department",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
