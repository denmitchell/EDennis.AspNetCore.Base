﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.IO;

namespace EDennis.Samples.Hr.InternalApi2.Migrations.FederalBackgroundCheckHistory
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo_history");

            migrationBuilder.CreateTable(
                name: "FederalBackgroundCheck",
                schema: "dbo_history",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    SysStart = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    SysEnd = table.Column<DateTime>(nullable: false),
                    SysUser = table.Column<string>(nullable: true),
                    SysUserNext = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederalBackgroundCheck", x => new { x.Id, x.SysStart });
                });
            migrationBuilder.SaveMappings();
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\FederalBackgroundCheckHistory_Insert.sql"));
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\FederalBackgroundCheckHistoryView.sql"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FederalBackgroundCheck",
                schema: "dbo_history");
        }
    }
}
