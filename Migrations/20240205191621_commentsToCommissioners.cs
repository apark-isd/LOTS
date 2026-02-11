using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class commentsToCommissioners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StatusTypeId",
                table: "Commissioner",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Commissioner",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Commissioner");

            migrationBuilder.AlterColumn<int>(
                name: "StatusTypeId",
                table: "Commissioner",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
