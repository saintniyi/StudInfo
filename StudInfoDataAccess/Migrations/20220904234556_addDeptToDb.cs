using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class addDeptToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depts",
                columns: table => new
                {
                    DeptCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeptName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    FacultyCode = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depts", x => x.DeptCode);
                    table.ForeignKey(
                        name: "FK_Depts_Faculties_FacultyCode",
                        column: x => x.FacultyCode,
                        principalTable: "Faculties",
                        principalColumn: "FacultyCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depts_FacultyCode",
                table: "Depts",
                column: "FacultyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depts");
        }
    }
}
