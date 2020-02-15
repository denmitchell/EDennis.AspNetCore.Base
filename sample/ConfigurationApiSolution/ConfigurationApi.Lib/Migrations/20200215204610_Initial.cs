using System;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConfigurationApi.Lib.Migrations
{
    public partial class Initial : Migration
    {
        [Up]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectSetting",
                columns: table => new
                {
                    ProjectName = table.Column<string>(nullable: false),
                    SettingKey = table.Column<string>(nullable: false),
                    SettingValue = table.Column<string>(nullable: true),
                    SysStart = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "(getdate())"),
                    SysEnd = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSetting", x => new { x.ProjectName, x.SettingKey });
                });
        }

        [Down]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectSetting");
        }
    }
}
