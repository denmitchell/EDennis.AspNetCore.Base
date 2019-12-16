using Microsoft.EntityFrameworkCore.Migrations;

namespace EDennis.Samples.DbContextConfigsApi.Lib.Migrations.DbContext2Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    SysUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    SysUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Name", "SysUser" },
                values: new object[] { 1, "Moe", "jack@hill.org" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Name", "SysUser" },
                values: new object[] { 2, "Larry", "jill@hill.org" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Name", "SysUser" },
                values: new object[] { 3, "Curly", "jack@hill.org" });

            migrationBuilder.InsertData(
                table: "Position",
                columns: new[] { "Id", "SysUser", "Title" },
                values: new object[] { 1, "jill@hill.org", "Manager" });

            migrationBuilder.InsertData(
                table: "Position",
                columns: new[] { "Id", "SysUser", "Title" },
                values: new object[] { 2, "jack@hill.org", "Employee" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Position");
        }
    }
}
