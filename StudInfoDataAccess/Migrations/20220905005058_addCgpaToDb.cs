using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class addCgpaToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cgpas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudNos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalUnits = table.Column<int>(type: "int", nullable: true),
                    TotalUnitWeight = table.Column<float>(type: "real", nullable: true),
                    CumulativeGPA = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cgpas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cgpas");
        }
    }
}
