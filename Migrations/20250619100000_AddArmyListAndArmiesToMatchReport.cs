using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarApi.Migrations
{
    /// <inheritdoc />
    public partial class AddArmyListAndArmiesToMatchReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArmyA",
                table: "MatchReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArmyB",
                table: "MatchReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ArmyLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Faction = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmyLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmyLists_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArmyLists_PlayerId",
                table: "ArmyLists",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmyLists");

            migrationBuilder.DropColumn(
                name: "ArmyA",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "ArmyB",
                table: "MatchReports");
        }
    }
}
