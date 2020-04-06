using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class addZVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZVote",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    DOCreating = table.Column<DateTimeOffset>(nullable: false),
                    DOEnd = table.Column<DateTimeOffset>(nullable: false),
                    SubmitterId = table.Column<string>(nullable: true),
                    IsSaveOnly = table.Column<bool>(nullable: false),
                    ItemType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZVote_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZVoteItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ForZVoteId = table.Column<Guid>(nullable: true),
                    ItemSource = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZVoteItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZVoteItem_ZVote_ForZVoteId",
                        column: x => x.ForZVoteId,
                        principalTable: "ZVote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZVote_SubmitterId",
                table: "ZVote",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_ZVoteItem_ForZVoteId",
                table: "ZVoteItem",
                column: "ForZVoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZVoteItem");

            migrationBuilder.DropTable(
                name: "ZVote");
        }
    }
}
