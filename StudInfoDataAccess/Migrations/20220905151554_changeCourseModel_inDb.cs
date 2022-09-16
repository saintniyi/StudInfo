using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class changeCourseModel_inDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Courses");
        }
    }
}
