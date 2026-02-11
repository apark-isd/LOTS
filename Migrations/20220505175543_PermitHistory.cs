using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class PermitHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "PickedPermitDate",
            //    table: "Permittee");

            //migrationBuilder.DropColumn(
            //    name: "RequestedPermitDate",
            //    table: "Permittee");

            migrationBuilder.AlterColumn<string>(
                name: "StatusTypeName",
                table: "StatusType",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PermitTypeName",
                table: "PermitType",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PermitteeTypeName",
                table: "PermitteeType",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Permittee",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Permittee",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeNo",
                table: "Permittee",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "RequestedDate",
            //    table: "Permittee",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "UpdatedStatusDate",
            //    table: "Permittee",
            //    nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayTypeName",
                table: "PayType",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LotName",
                table: "Lot",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentAbrv",
                table: "Department",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PermitHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermitteeId = table.Column<int>(nullable: false),
                    LotId = table.Column<int>(nullable: false),
                    PermitNo = table.Column<string>(nullable: true),
                    PermitTypeId = table.Column<int>(nullable: false),
                    PermitteeTypeId = table.Column<int>(nullable: false),
                    PayTypeId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermitHistory_Lot_LotId",
                        column: x => x.LotId,
                        principalTable: "Lot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermitHistory_PayType_PayTypeId",
                        column: x => x.PayTypeId,
                        principalTable: "PayType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermitHistory_PermitType_PermitTypeId",
                        column: x => x.PermitTypeId,
                        principalTable: "PermitType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermitHistory_Permittee_PermitteeId",
                        column: x => x.PermitteeId,
                        principalTable: "Permittee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermitHistory_PermitteeType_PermitteeTypeId",
                        column: x => x.PermitteeTypeId,
                        principalTable: "PermitteeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermitHistory_LotId",
                table: "PermitHistory",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitHistory_PayTypeId",
                table: "PermitHistory",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitHistory_PermitTypeId",
                table: "PermitHistory",
                column: "PermitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitHistory_PermitteeId",
                table: "PermitHistory",
                column: "PermitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitHistory_PermitteeTypeId",
                table: "PermitHistory",
                column: "PermitteeTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermitHistory");

            //migrationBuilder.DropColumn(
            //    name: "RequestedDate",
            //    table: "Permittee");

            //migrationBuilder.DropColumn(
            //    name: "UpdatedStatusDate",
            //    table: "Permittee");

            migrationBuilder.AlterColumn<string>(
                name: "StatusTypeName",
                table: "StatusType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PermitTypeName",
                table: "PermitType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PermitteeTypeName",
                table: "PermitteeType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Permittee",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Permittee",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeNo",
                table: "Permittee",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "PickedPermitDate",
            //    table: "Permittee",
            //    type: "datetime2",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "RequestedPermitDate",
            //    table: "Permittee",
            //    type: "datetime2",
            //    nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayTypeName",
                table: "PayType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LotName",
                table: "Lot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentAbrv",
                table: "Department",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
