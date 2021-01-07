using Microsoft.EntityFrameworkCore.Migrations;

namespace DevixonApi.Migrations
{
    public partial class FacebookUserColumnToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FacebookUser",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUser",
                table: "User");
        }
    }
}
