using Microsoft.EntityFrameworkCore.Migrations;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Red = table.Column<int>(nullable: false),
                    Green = table.Column<int>(nullable: false),
                    Blue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "Blue", "Green", "Name", "Red" },
                values: new object[] { 1, 0, 0, "Black", 0 });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "Blue", "Green", "Name", "Red" },
                values: new object[] { 2, 255, 255, "White", 255 });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "Blue", "Green", "Name", "Red" },
                values: new object[] { 3, 127, 127, "Grey", 127 });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "Blue", "Green", "Name", "Red" },
                values: new object[] { 4, 0, 0, "Red", 255 });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "Blue", "Green", "Name", "Red" },
                values: new object[] { 5, 0, 255, "Green", 0 });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "Blue", "Green", "Name", "Red" },
                values: new object[] { 6, 255, 0, "Blue", 0 });

            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "Id", "Blue", "Green", "Name", "Red" },
                values: new object[] { 7, 0, 255, "Yellow", 255 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Color");
        }
    }
}
