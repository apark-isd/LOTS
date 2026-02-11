using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOTS3.Migrations
{
    public partial class AddingIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Department",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DepartmentNo = table.Column<int>(nullable: false),
            //        DepartmentAbrv = table.Column<string>(nullable: false),
            //        FundOrg = table.Column<int>(nullable: false),
            //        DepartmentName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Department", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Lot",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        LotNo = table.Column<string>(nullable: true),
            //        LotName = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Lot", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PayType",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PayTypeName = table.Column<string>(nullable: false),
            //        PayTypeAmount = table.Column<decimal>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PayType", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermitteeType",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PermitteeTypeNo = table.Column<int>(nullable: false),
            //        PermitteeTypeName = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermitteeType", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermitType",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PermitTypeNo = table.Column<int>(nullable: false),
            //        PermitTypeName = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermitType", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "StatusType",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        StatusTypeNo = table.Column<int>(nullable: false),
            //        StatusTypeName = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_StatusType", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateTable(
            //    name: "Coordinator",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EmployeeNo = table.Column<string>(nullable: false),
            //        FirstName = table.Column<string>(maxLength: 50, nullable: false),
            //        LastName = table.Column<string>(maxLength: 50, nullable: false),
            //        PhoneNumber = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true),
            //        Division = table.Column<string>(nullable: true),
            //        DepartmentAbrv = table.Column<string>(nullable: false),
            //        DepartmentId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Coordinator", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Coordinator_Department_DepartmentId",
            //            column: x => x.DepartmentId,
            //            principalTable: "Department",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Permittee",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EmployeeNo = table.Column<string>(nullable: false),
            //        FirstName = table.Column<string>(maxLength: 50, nullable: false),
            //        LastName = table.Column<string>(maxLength: 50, nullable: false),
            //        PhoneNumber = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true),
            //        Division = table.Column<string>(nullable: true),
            //        DepartmentId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Permittee", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Permittee_Department_DepartmentId",
            //            column: x => x.DepartmentId,
            //            principalTable: "Department",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Permit",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PermitteeId = table.Column<int>(nullable: false),
            //        LotId = table.Column<int>(nullable: false),
            //        PermitNo = table.Column<string>(nullable: true),
            //        PermitTypeId = table.Column<int>(nullable: false),
            //        PermitteeTypeId = table.Column<int>(nullable: false),
            //        PayTypeId = table.Column<int>(nullable: false),
            //        StatusTypeId = table.Column<int>(nullable: false),
            //        StartDate = table.Column<DateTime>(nullable: true),
            //        EndDate = table.Column<DateTime>(nullable: true),
            //        Comments = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Permit", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Permit_Lot_LotId",
            //            column: x => x.LotId,
            //            principalTable: "Lot",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Permit_PayType_PayTypeId",
            //            column: x => x.PayTypeId,
            //            principalTable: "PayType",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Permit_PermitType_PermitTypeId",
            //            column: x => x.PermitTypeId,
            //            principalTable: "PermitType",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Permit_Permittee_PermitteeId",
            //            column: x => x.PermitteeId,
            //            principalTable: "Permittee",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Permit_PermitteeType_PermitteeTypeId",
            //            column: x => x.PermitteeTypeId,
            //            principalTable: "PermitteeType",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Permit_StatusType_StatusTypeId",
            //            column: x => x.StatusTypeId,
            //            principalTable: "StatusType",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinator_DepartmentId",
                table: "Coordinator",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Permit_LotId",
                table: "Permit",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_Permit_PayTypeId",
                table: "Permit",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permit_PermitTypeId",
                table: "Permit",
                column: "PermitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permit_PermitteeId",
                table: "Permit",
                column: "PermitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permit_PermitteeTypeId",
                table: "Permit",
                column: "PermitteeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permit_StatusTypeId",
                table: "Permit",
                column: "StatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permittee_DepartmentId",
                table: "Permittee",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Coordinator");

            migrationBuilder.DropTable(
                name: "Permit");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Lot");

            migrationBuilder.DropTable(
                name: "PayType");

            migrationBuilder.DropTable(
                name: "PermitType");

            migrationBuilder.DropTable(
                name: "Permittee");

            migrationBuilder.DropTable(
                name: "PermitteeType");

            migrationBuilder.DropTable(
                name: "StatusType");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
