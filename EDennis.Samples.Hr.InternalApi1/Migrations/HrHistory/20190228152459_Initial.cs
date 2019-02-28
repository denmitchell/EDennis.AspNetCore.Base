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
                name: "EmployeePosition",
                schema: "dbo_history",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false),
                    PositionId = table.Column<int>(nullable: false),
                    SysStart = table.Column<DateTime>(nullable: false),
                    SysEnd = table.Column<DateTime>(nullable: false),
                    SysUser = table.Column<string>(nullable: true),
                    SysUserNext = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePosition", x => new { x.EmployeeId, x.PositionId, x.SysStart });
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee",
                schema: "dbo_history");

            migrationBuilder.DropTable(
                name: "EmployeePosition",
                schema: "dbo_history");

            migrationBuilder.DropTable(
                name: "Position",
                schema: "dbo_history");
        }
    }
}
