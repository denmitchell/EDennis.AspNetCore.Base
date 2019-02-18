using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.IO;

namespace EDennis.Samples.Hr.InternalApi2.Migrations.AgencyOnlineCheck
{
    public partial class AgencyOnlineCheck_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();
            migrationBuilder.CreateTable(
                name: "AgencyOnlineCheck",
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
                    table.PrimaryKey("PK_AgencyOnlineCheck", x => x.Id);
                });


            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\AgencyOnlineCheck_Insert.sql"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropMaintenanceProcedures();
            migrationBuilder.DropTestJsonTableSupport();

            migrationBuilder.DropTable(
                name: "AgencyOnlineCheck");
        }
    }
}
