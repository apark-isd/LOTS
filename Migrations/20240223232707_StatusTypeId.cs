using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class StatusTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<bool>(
            //    name: "YesEmployee",
            //    table: "VehicleLiabilityWaiver",
            //    nullable: false,
            //    oldClrType: typeof(bool),
            //    oldType: "bit",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<bool>(
            //    name: "NoCompany",
            //    table: "VehicleLiabilityWaiver",
            //    nullable: false,
            //    oldClrType: typeof(bool),
            //    oldType: "bit",
            //    oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusTypeId",
                table: "VehicleLiabilityWaiver",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_VehicleLiabilityWaiver_StatusTypeId",
            //    table: "VehicleLiabilityWaiver",
            //    column: "StatusTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLiabilityWaiver_StatusType_StatusTypeId",
                table: "VehicleLiabilityWaiver",
                column: "StatusTypeId",
                principalTable: "StatusType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleLiabilityWaiver_StatusType_StatusTypeId",
                table: "VehicleLiabilityWaiver");

            //migrationBuilder.DropIndex(
            //    name: "IX_VehicleLiabilityWaiver_StatusTypeId",
            //    table: "VehicleLiabilityWaiver");

            migrationBuilder.DropColumn(
                name: "StatusTypeId",
                table: "VehicleLiabilityWaiver");

            //migrationBuilder.AlterColumn<bool>(
            //    name: "YesEmployee",
            //    table: "VehicleLiabilityWaiver",
            //    type: "bit",
            //    nullable: true,
            //    oldClrType: typeof(bool));

            //migrationBuilder.AlterColumn<bool>(
            //    name: "NoCompany",
            //    table: "VehicleLiabilityWaiver",
            //    type: "bit",
            //    nullable: true,
            //    oldClrType: typeof(bool));
        }
    }
}
