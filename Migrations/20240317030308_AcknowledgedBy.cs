using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class AcknowledgedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Approvedby",
                table: "VehicleLiabilityWaiver",
                newName: "ApprovedBy");

            migrationBuilder.AddColumn<string>(
                name: "AcknowledgedBy",
                table: "VehicleLiabilityWaiver",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcknowledgedBy",
                table: "VehicleLiabilityWaiver");

            migrationBuilder.RenameColumn(
                name: "ApprovedBy",
                table: "VehicleLiabilityWaiver",
                newName: "Approvedby");
        }
    }
}
