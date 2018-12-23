using System;
using EDennis.MigrationsExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EDennis.Samples.TodoApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateMaintenanceProcedures();
            migrationBuilder.CreateTestJsonTableSupport();
            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    PercentComplete = table.Column<decimal>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    SysStart = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    SysEnd = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "DueDate", "PercentComplete", "Title" },
                values: new object[] { 1, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, "Take out the garbage" });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "DueDate", "PercentComplete", "Title" },
                values: new object[] { 2, new DateTime(2018, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 50m, "Clean the attic" });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "DueDate", "PercentComplete", "Title" },
                values: new object[] { 3, new DateTime(2018, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, "Clean the basement" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropMaintenanceProcedures();
            migrationBuilder.DropTestJsonTableSupport();
            migrationBuilder.DropTable(
                name: "Task");
        }
    }
}
