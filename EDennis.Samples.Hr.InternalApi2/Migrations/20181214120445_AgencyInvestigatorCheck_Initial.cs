using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;

namespace EDennis.Samples.Hr.InternalApi2.Migrations
{
    public partial class AgencyInvestigatorCheck_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();
            migrationBuilder.CreateTable(
                name: "AgencyInvestigatorCheck",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(maxLength: 100, nullable: true),
                    SysStart = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    SysEnd = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyInvestigatorCheck", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AgencyInvestigatorCheck",
                columns: new[] { "Id", "DateCompleted", "EmployeeId", "Status" },
                values: new object[] { 1, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Pass" });

            migrationBuilder.InsertData(
                table: "AgencyInvestigatorCheck",
                columns: new[] { "Id", "DateCompleted", "EmployeeId", "Status" },
                values: new object[] { 2, new DateTime(2018, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Pass" });

            migrationBuilder.InsertData(
                table: "AgencyInvestigatorCheck",
                columns: new[] { "Id", "DateCompleted", "EmployeeId", "Status" },
                values: new object[] { 3, new DateTime(2018, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Fail" });

            migrationBuilder.InsertData(
                table: "AgencyInvestigatorCheck",
                columns: new[] { "Id", "DateCompleted", "EmployeeId", "Status" },
                values: new object[] { 4, new DateTime(2018, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Pass" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropMaintenanceProcedures();
            migrationBuilder.DropTestJsonTableSupport();
            migrationBuilder.DropTable(
                name: "AgencyInvestigatorCheck");
        }
    }
}
