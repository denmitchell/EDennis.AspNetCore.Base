using System;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.IO;

namespace Colors2.Migrations
{
    public partial class KK12GjP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();

            migrationBuilder.CreateSequence<int>(
                name: "seqRgb");

            migrationBuilder.CreateTable(
                name: "Rgb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR seqRgb"),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Red = table.Column<int>(nullable: false),
                    Green = table.Column<int>(nullable: false),
                    Blue = table.Column<int>(nullable: false),
                    SysUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkRgb", x => x.Id);
                });
            migrationBuilder.SaveMappings();
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\RgbInserts.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\vwHsl.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\HslByColorName.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\RgbJsonByColorName.sql"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rgb");

            migrationBuilder.DropSequence(
                name: "seqRgb");

            migrationBuilder.DropTestJsonTableSupport();
            migrationBuilder.DropMaintenanceProcedures();
        }
    }
}
