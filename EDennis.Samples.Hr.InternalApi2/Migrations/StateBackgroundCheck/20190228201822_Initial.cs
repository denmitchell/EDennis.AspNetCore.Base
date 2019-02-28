using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.IO;

namespace EDennis.Samples.Hr.InternalApi2.Migrations.StateBackgroundCheck
{
    public partial class Initial : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();

            migrationBuilder.CreateTable(
                name: "StateBackgroundCheck",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateBackgroundCheck", x => x.Id);
                });
            migrationBuilder.SaveMappings();
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\StateBackgroundCheck_Insert.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\StateBackgroundCheckView.sql"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateBackgroundCheck");
            migrationBuilder.DropTestJsonTableSupport();
            migrationBuilder.DropMaintenanceProcedures();
        }
    }
}
