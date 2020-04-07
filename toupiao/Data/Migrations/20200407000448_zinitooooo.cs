using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class zinitooooo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZUserVote",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ZVoteItemId = table.Column<Guid>(nullable: false),
                    VoterId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZUserVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZUserVote_AspNetUsers_VoterId",
                        column: x => x.VoterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZUserVote_ZVoteItem_ZVoteItemId",
                        column: x => x.ZVoteItemId,
                        principalTable: "ZVoteItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZUserVote_VoterId",
                table: "ZUserVote",
                column: "VoterId");

            migrationBuilder.CreateIndex(
                name: "IX_ZUserVote_ZVoteItemId",
                table: "ZUserVote",
                column: "ZVoteItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZUserVote");
        }
    }
}
