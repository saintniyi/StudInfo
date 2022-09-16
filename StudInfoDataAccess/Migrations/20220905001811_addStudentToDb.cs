using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class addStudentToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudNos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    PhoneNos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Pix = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DeptCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    DegreeProgramName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DegreeTitleToBeAwarded = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: true),
                    LatestEnrollGrpCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GPA = table.Column<float>(type: "real", nullable: true),
                    CGPA = table.Column<float>(type: "real", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhotoBackupPath = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudNos);
                    table.ForeignKey(
                        name: "FK_Students_Depts_DeptCode",
                        column: x => x.DeptCode,
                        principalTable: "Depts",
                        principalColumn: "DeptCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_DeptCode",
                table: "Students",
                column: "DeptCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
