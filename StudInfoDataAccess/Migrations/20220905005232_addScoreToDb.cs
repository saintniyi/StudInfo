using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class addScoreToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudNos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mark = table.Column<float>(type: "real", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Processed = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GradeLetter = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Weight = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Scores_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_Grades_GradeLetter",
                        column: x => x.GradeLetter,
                        principalTable: "Grades",
                        principalColumn: "GradeLetter",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_Students_StudNos",
                        column: x => x.StudNos,
                        principalTable: "Students",
                        principalColumn: "StudNos",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_CourseCode",
                table: "Scores",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_GradeLetter",
                table: "Scores",
                column: "GradeLetter");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_StudNos",
                table: "Scores",
                column: "StudNos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores");
        }
    }
}
