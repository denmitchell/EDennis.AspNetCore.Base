using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;

namespace EDennis.Samples.Hr.InternalApi2.Migrations.FederalBackgroundCheck
{
    public partial class FederalBackgroundCheck_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();
            migrationBuilder.CreateTable(
                name: "FederalBackgroundCheck",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederalBackgroundCheck", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FederalBackgroundCheck",
                columns: new[] { "Id", "DateCompleted", "EmployeeId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Pass" },
                    { 2, new DateTime(2018, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Pass" },
                    { 3, new DateTime(2018, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Fail" },
                    { 4, new DateTime(2018, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Pass" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropMaintenanceProcedures();
            migrationBuilder.DropTestJsonTableSupport();
            migrationBuilder.DropTable(
                name: "FederalBackgroundCheck");
        }
    }
}
