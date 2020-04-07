using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class zinit0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZVoteItem_ZVote_ZVote",
                table: "ZVoteItem");

            migrationBuilder.DropIndex(
                name: "IX_ZVoteItem_ZVote",
                table: "ZVoteItem");

            migrationBuilder.DropColumn(
                name: "ZVote",
                table: "ZVoteItem");

            migrationBuilder.AddColumn<Guid>(
                name: "ZVoteId",
                table: "ZVoteItem",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ZVoteItem_ZVoteId",
                table: "ZVoteItem",
                column: "ZVoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZVoteItem_ZVote_ZVoteId",
                table: "ZVoteItem",
                column: "ZVoteId",
                principalTable: "ZVote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZVoteItem_ZVote_ZVoteId",
                table: "ZVoteItem");

            migrationBuilder.DropIndex(
                name: "IX_ZVoteItem_ZVoteId",
                table: "ZVoteItem");

            migrationBuilder.DropColumn(
                name: "ZVoteId",
                table: "ZVoteItem");

            migrationBuilder.AddColumn<Guid>(
                name: "ZVote",
                table: "ZVoteItem",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZVoteItem_ZVote",
                table: "ZVoteItem",
                column: "ZVote");

            migrationBuilder.AddForeignKey(
                name: "FK_ZVoteItem_ZVote_ZVote",
                table: "ZVoteItem",
                column: "ZVote",
                principalTable: "ZVote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
