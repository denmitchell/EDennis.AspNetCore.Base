using System;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Colors2.Migrations
{
    public partial class KK1A8lN : Migration
    {
        [Up]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    DateAdded = table.Column<DateTime>(nullable: false),
                    SysStart = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    SysEnd = table.Column<DateTime>(nullable: false, defaultValueSql: "(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))"),
                    SysUserNext = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkRgb", x => x.Id);
                });
        }

        [Down]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rgb");

            migrationBuilder.DropSequence(
                name: "seqRgb");
        }
    }
}
