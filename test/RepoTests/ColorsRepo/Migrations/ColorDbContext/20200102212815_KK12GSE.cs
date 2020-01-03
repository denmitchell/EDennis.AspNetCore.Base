using System;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.IO;
using EDennis.AspNetCore.Base.EntityFramework;

namespace Colors.Migrations
{
    public partial class KK12GSE : Migration
    {
        [Up]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateMaintenanceProcedures();
            //migrationBuilder.CreateTestJsonTableSupport();

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "seqColor");

            migrationBuilder.CreateTable(
                name: "Color",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR seqColor"),
                    SysStart = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    SysEnd = table.Column<DateTime>(nullable: false),
                    SysUser = table.Column<string>(nullable: true),
                    SysUserNext = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            //migrationBuilder.SaveMappings();
            //migrationBuilder.CreateSqlServerTemporalTables();
            //migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\Initial_Insert.sql"));

        }

        [Down]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Color",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "seqColor");

            //migrationBuilder.DropTestJsonTableSupport();
            //migrationBuilder.DropMaintenanceProcedures();
        }
    }
}
