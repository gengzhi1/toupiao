using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class lllllllllllllllllllll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZUserVote_AspNetUsers_VoterId",
                table: "ZUserVote");

            migrationBuilder.DropIndex(
                name: "IX_ZUserVote_VoterId",
                table: "ZUserVote");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ZUserVote_VoterId",
                table: "ZUserVote",
                column: "VoterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZUserVote_AspNetUsers_VoterId",
                table: "ZUserVote",
                column: "VoterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
