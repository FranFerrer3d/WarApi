using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerAndMatchReportEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerA",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "PlayerB",
                table: "MatchReports");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerAId",
                table: "MatchReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerBId",
                table: "MatchReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MatchReports_PlayerAId",
                table: "MatchReports",
                column: "PlayerAId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchReports_PlayerBId",
                table: "MatchReports",
                column: "PlayerBId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchReports_Players_PlayerAId",
                table: "MatchReports",
                column: "PlayerAId",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchReports_Players_PlayerBId",
                table: "MatchReports",
                column: "PlayerBId",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchReports_Players_PlayerAId",
                table: "MatchReports");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchReports_Players_PlayerBId",
                table: "MatchReports");

            migrationBuilder.DropIndex(
                name: "IX_MatchReports_PlayerAId",
                table: "MatchReports");

            migrationBuilder.DropIndex(
                name: "IX_MatchReports_PlayerBId",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "PlayerAId",
                table: "MatchReports");

            migrationBuilder.DropColumn(
                name: "PlayerBId",
                table: "MatchReports");

            migrationBuilder.AddColumn<string>(
                name: "PlayerA",
                table: "MatchReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlayerB",
                table: "MatchReports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
