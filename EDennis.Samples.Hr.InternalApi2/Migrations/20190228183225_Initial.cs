﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.IO;

namespace EDennis.Samples.Hr.InternalApi2.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();

            migrationBuilder.CreateTable(
                name: "AgencyInvestigatorCheck",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    table.PrimaryKey("PK_AgencyInvestigatorCheck", x => x.Id);
                });

            migrationBuilder.SaveMappings();
            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\AgencyInvestigatorCheck_Insert.sql"));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgencyInvestigatorCheck");

            migrationBuilder.DropTestJsonTableSupport();
            migrationBuilder.DropMaintenanceProcedures();
        }
    }
}
