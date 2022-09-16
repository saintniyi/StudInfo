using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class addSequenceToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "GetSequenceNos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "GetSequenceNos");
        }
    }
}
