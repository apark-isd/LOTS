using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class vehicleLiabilityWaiver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleLiabilityWaiver",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateRequested = table.Column<DateTime>(nullable: true),
                    ReasonForRequest = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    YesEmployee = table.Column<bool>(nullable: true),
                    NoCompany = table.Column<bool>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    LotId = table.Column<int>(nullable: false),
                    PermitNo = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    MakeModel = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    LicensePlateNumber = table.Column<string>(nullable: true),
                    Signature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLiabilityWaiver", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleLiabilityWaiver_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleLiabilityWaiver_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_VehicleLiabilityWaiver_DepartmentId",
            //    table: "VehicleLiabilityWaiver",
            //    column: "DepartmentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_VehicleLiabilityWaiver_LotId",
            //    table: "VehicleLiabilityWaiver",
            //    column: "LotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleLiabilityWaiver");
        }
    }
}
