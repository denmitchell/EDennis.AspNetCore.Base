using Microsoft.EntityFrameworkCore.Migrations;

namespace DbContextInterceptorApi.Lib.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "seqPerson");

            migrationBuilder.CreateSequence<int>(
                name: "seqPosition");

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR seqPerson"),
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
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR seqPosition"),
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
                values: new object[,]
                {
                    { -999001, "Mike", "jack@hill.org" },
                    { -999002, "Carol", "jill@hill.org" },
                    { -999003, "Greg", "jack@hill.org" }
                });

            migrationBuilder.InsertData(
                table: "Position",
                columns: new[] { "Id", "SysUser", "Title" },
                values: new object[,]
                {
                    { -999001, "jill@hill.org", "President" },
                    { -999002, "jack@hill.org", "Vice-president" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropSequence(
                name: "seqPerson");

            migrationBuilder.DropSequence(
                name: "seqPosition");
        }
    }
}
