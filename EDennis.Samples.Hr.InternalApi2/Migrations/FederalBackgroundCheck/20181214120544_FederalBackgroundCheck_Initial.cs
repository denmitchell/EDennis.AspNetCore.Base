using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.IO;

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


            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\FederalBackgroundCheck_Insert.sql"));
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
