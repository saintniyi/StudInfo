using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class addEnrollmentToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudNos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Processed = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudNos",
                        column: x => x.StudNos,
                        principalTable: "Students",
                        principalColumn: "StudNos",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseCode",
                table: "Enrollments",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudNos",
                table: "Enrollments",
                column: "StudNos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");
        }
    }
}
