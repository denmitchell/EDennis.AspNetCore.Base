using System;
using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using EDennis.MigrationsExtensions;

namespace ConfigurationApi.Lib.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();

            migrationBuilder.CreateSequence<int>(
                name: "seqProject");

            migrationBuilder.CreateSequence<int>(
                name: "seqSetting");

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSetting",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    SettingId = table.Column<int>(nullable: false),
                    SettingValue = table.Column<string>(nullable: true),
                    SysStart = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    SysEnd = table.Column<DateTime>(nullable: false, defaultValueSql: "(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSetting", x => new { x.ProjectId, x.SettingId });
                    table.ForeignKey(
                        name: "fk_ProjectSetting_Project",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ProjectSetting_Setting",
                        column: x => x.SettingId,
                        principalTable: "Setting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSetting_SettingId",
                table: "ProjectSetting",
                column: "SettingId");

            migrationBuilder.SaveMappings();
            migrationBuilder.CreateSqlServerTemporalTables();
            migrationBuilder.Sql(File.ReadAllText("MigrationsSql\\PostUp\\01_CreateTableType.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsSql\\PostUp\\02_GetLastModified.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsSql\\PostUp\\02_SaveSettings.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsSql\\PostUp\\02_vwProjectSetting.sql"));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectSetting");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropSequence(
                name: "seqProject");

            migrationBuilder.DropSequence(
                name: "seqSetting");

            migrationBuilder.DropMaintenanceProcedures();
            migrationBuilder.DropTestJsonTableSupport();
        }
    }
}
