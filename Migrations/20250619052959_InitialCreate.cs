using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerA = table.Column<string>(type: "text", nullable: false),
                    PlayerB = table.Column<string>(type: "text", nullable: false),
                    ListA = table.Column<string>(type: "text", nullable: false),
                    ListB = table.Column<string>(type: "text", nullable: false),
                    ExpectedA = table.Column<int>(type: "integer", nullable: false),
                    ExpectedB = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Map = table.Column<string>(type: "text", nullable: false),
                    Deployment = table.Column<string>(type: "text", nullable: false),
                    PrimaryMission = table.Column<string>(type: "text", nullable: false),
                    SecondaryA = table.Column<string>(type: "text", nullable: false),
                    SecondaryB = table.Column<string>(type: "text", nullable: false),
                    MagicA = table.Column<int[]>(type: "integer[]", nullable: false),
                    MagicB = table.Column<int[]>(type: "integer[]", nullable: false),
                    KillsA = table.Column<int>(type: "integer", nullable: false),
                    KillsB = table.Column<int>(type: "integer", nullable: false),
                    PrimaryResult = table.Column<int>(type: "integer", nullable: false),
                    SecondaryWinA = table.Column<bool>(type: "boolean", nullable: false),
                    SecondaryWinB = table.Column<bool>(type: "boolean", nullable: false),
                    FinalScoreA = table.Column<int>(type: "integer", nullable: false),
                    FinalScoreB = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellidos = table.Column<string>(type: "text", nullable: false),
                    Alias = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Contraseña = table.Column<string>(type: "text", nullable: false),
                    Foto = table.Column<string>(type: "text", nullable: true),
                    Equipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchReports");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
