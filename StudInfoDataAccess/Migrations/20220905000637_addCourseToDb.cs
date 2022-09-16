using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class addCourseToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CourseUnit = table.Column<int>(type: "int", nullable: false),
                    DeptCode = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseCode);
                    table.ForeignKey(
                        name: "FK_Courses_Depts_DeptCode",
                        column: x => x.DeptCode,
                        principalTable: "Depts",
                        principalColumn: "DeptCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DeptCode",
                table: "Courses",
                column: "DeptCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
