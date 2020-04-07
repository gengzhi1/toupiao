using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class deleteVoteType00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZUserVote_ZVoteItem_ZVoteItemId",
                table: "ZUserVote");

            migrationBuilder.DropTable(
                name: "ZVoteItem");

            migrationBuilder.DropIndex(
                name: "IX_ZUserVote_ZVoteItemId",
                table: "ZUserVote");

            migrationBuilder.DropColumn(
                name: "ZVoteItemId",
                table: "ZUserVote");

            migrationBuilder.AddColumn<bool>(
                name: "IsLegal",
                table: "ZVote",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VoteItem",
                table: "ZUserVote",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ZVoteId",
                table: "ZUserVote",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLegal",
                table: "ZVote");

            migrationBuilder.DropColumn(
                name: "VoteItem",
                table: "ZUserVote");

            migrationBuilder.DropColumn(
                name: "ZVoteId",
                table: "ZUserVote");

            migrationBuilder.AddColumn<Guid>(
                name: "ZVoteItemId",
                table: "ZUserVote",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ZVoteItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageFileName = table.Column<string>(type: "text", nullable: true),
                    ZVoteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZVoteItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZVoteItem_ZVote_ZVoteId",
                        column: x => x.ZVoteId,
                        principalTable: "ZVote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZUserVote_ZVoteItemId",
                table: "ZUserVote",
                column: "ZVoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ZVoteItem_ZVoteId",
                table: "ZVoteItem",
                column: "ZVoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZUserVote_ZVoteItem_ZVoteItemId",
                table: "ZUserVote",
                column: "ZVoteItemId",
                principalTable: "ZVoteItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
