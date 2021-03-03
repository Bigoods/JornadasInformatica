using Microsoft.EntityFrameworkCore.Migrations;

namespace Leaderboard_JEI.Data.Migrations
{
    public partial class perfil1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Perfils",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Perfils");
        }
    }
}
