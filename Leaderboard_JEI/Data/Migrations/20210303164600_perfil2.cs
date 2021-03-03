using Microsoft.EntityFrameworkCore.Migrations;

namespace Leaderboard_JEI.Data.Migrations
{
    public partial class perfil2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Perfils");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Perfils",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Perfils");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Perfils",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
