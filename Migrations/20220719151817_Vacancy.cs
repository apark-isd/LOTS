using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class Vacancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacancy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LotId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    CheckTimeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacancy_CheckTime_CheckTimeId",
                        column: x => x.CheckTimeId,
                        principalTable: "CheckTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vacancy_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vacancy_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_CheckTimeId",
                table: "Vacancy",
                column: "CheckTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_LocationId",
                table: "Vacancy",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_LotId",
                table: "Vacancy",
                column: "LotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vacancy");
        }
    }
}
