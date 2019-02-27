using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EDennis.Samples.Hr.InternalApi1.Migrations.HrHistory
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo_history");

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "dbo_history",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    SysStart = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    SysEnd = table.Column<DateTime>(nullable: false),
                    SysUser = table.Column<string>(nullable: true),
                    SysUserNext = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => new { x.Id, x.SysStart });
                });

            migrationBuilder.CreateTable(
                name: "Position",
                schema: "dbo_history",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    SysStart = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 60, nullable: true),
                    IsManager = table.Column<bool>(nullable: false),
                    SysEnd = table.Column<DateTime>(nullable: false),
                    SysUser = table.Column<string>(nullable: true),
                    SysUserNext = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => new { x.Id, x.SysStart });
                });

            migrationBuilder.CreateTable(
                name: "EmployeePosition",
                schema: "dbo_history",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false),
                    PositionId = table.Column<int>(nullable: false),
                    SysStart = table.Column<DateTime>(nullable: false),
                    SysEnd = table.Column<DateTime>(nullable: false),
                    SysUser = table.Column<string>(nullable: true),
                    SysUserNext = table.Column<string>(nullable: true),
                    EmployeeId1 = table.Column<int>(nullable: true),
                    EmployeeSysStart = table.Column<DateTime>(nullable: true),
                    PositionId1 = table.Column<int>(nullable: true),
                    PositionSysStart = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePosition", x => new { x.EmployeeId, x.PositionId, x.SysStart });
                    table.ForeignKey(
                        name: "FK_EmployeePosition_Employee_EmployeeId1_EmployeeSysStart",
                        columns: x => new { x.EmployeeId1, x.EmployeeSysStart },
                        principalSchema: "dbo_history",
                        principalTable: "Employee",
                        principalColumns: new[] { "Id", "SysStart" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePosition_Position_PositionId1_PositionSysStart",
                        columns: x => new { x.PositionId1, x.PositionSysStart },
                        principalSchema: "dbo_history",
                        principalTable: "Position",
                        principalColumns: new[] { "Id", "SysStart" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePosition_EmployeeId1_EmployeeSysStart",
                schema: "dbo_history",
                table: "EmployeePosition",
                columns: new[] { "EmployeeId1", "EmployeeSysStart" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePosition_PositionId1_PositionSysStart",
                schema: "dbo_history",
                table: "EmployeePosition",
                columns: new[] { "PositionId1", "PositionSysStart" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePosition",
                schema: "dbo_history");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "dbo_history");

            migrationBuilder.DropTable(
                name: "Position",
                schema: "dbo_history");
        }
    }
}
