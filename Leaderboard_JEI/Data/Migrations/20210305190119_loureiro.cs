using Microsoft.EntityFrameworkCore.Migrations;

namespace Leaderboard_JEI.Data.Migrations
{
    public partial class loureiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rifa1",
                table: "Perfils");

            migrationBuilder.DropColumn(
                name: "Rifa2",
                table: "Perfils");

            migrationBuilder.DropColumn(
                name: "Rifa3",
                table: "Perfils");

            migrationBuilder.DropColumn(
                name: "Rifa4",
                table: "Perfils");

            migrationBuilder.CreateTable(
                name: "Rifas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    NumRifa = table.Column<int>(nullable: false),
                    TipoRifa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rifas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rifas");

            migrationBuilder.AddColumn<int>(
                name: "Rifa1",
                table: "Perfils",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rifa2",
                table: "Perfils",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rifa3",
                table: "Perfils",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rifa4",
                table: "Perfils",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
