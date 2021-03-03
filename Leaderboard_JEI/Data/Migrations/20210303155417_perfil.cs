using Microsoft.EntityFrameworkCore.Migrations;

namespace Leaderboard_JEI.Data.Migrations
{
    public partial class perfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perfils",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pontos = table.Column<int>(nullable: false),
                    Rifa1 = table.Column<int>(nullable: false),
                    Rifa2 = table.Column<int>(nullable: false),
                    Rifa3 = table.Column<int>(nullable: false),
                    Rifa4 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfils", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perfils");
        }
    }
}
