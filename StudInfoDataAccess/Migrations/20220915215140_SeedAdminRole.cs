using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudInfoDataAccess.Migrations
{
    public partial class SeedAdminRole : Migration
    {

        private string AdminRoleId = Guid.NewGuid().ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedRolesSQL(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }


        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"INSERT INTO [dbo].[AspNetRoles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
            VALUES ('{AdminRoleId}', 'Admin', 'ADMIN', null);");
        }



    }
}
