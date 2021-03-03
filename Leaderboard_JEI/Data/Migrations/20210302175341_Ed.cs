using Microsoft.EntityFrameworkCore.Migrations;

namespace Leaderboard_JEI.Data.Migrations
{
    public partial class Ed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PontuacaoDiaria",
                table: "Participante",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PontuacaoDiaria",
                table: "Participante");
        }
    }
}
