using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class Allocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PayTypeAmount",
                table: "PayType",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Allocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllocationNo = table.Column<int>(nullable: true),
                    IssuedNo = table.Column<int>(nullable: true),
                    PlusMinusAllocation = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    LotId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocation_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Allocation_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_DepartmentId",
                table: "Allocation",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_LotId",
                table: "Allocation",
                column: "LotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allocation");

            migrationBuilder.AlterColumn<decimal>(
                name: "PayTypeAmount",
                table: "PayType",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
