using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class AddKeycardNoToPermit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "StatusTypeId",
            //    table: "Permit",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeycardNo",
                table: "Permit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeycardNo",
                table: "Permit");

            //migrationBuilder.AlterColumn<int>(
            //    name: "StatusTypeId",
            //    table: "Permit",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int));
        }
    }
}
