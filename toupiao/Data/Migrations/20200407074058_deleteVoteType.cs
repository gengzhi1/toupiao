using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class deleteVoteType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "ZVote");

            migrationBuilder.DropColumn(
                name: "MaxItemCountpu",
                table: "ZVote");

            migrationBuilder.AddColumn<string>(
                name: "XuanxiangA",
                table: "ZVote",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XuanxiangB",
                table: "ZVote",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XuanxiangC",
                table: "ZVote",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XuanxiangD",
                table: "ZVote",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XuanxiangA",
                table: "ZVote");

            migrationBuilder.DropColumn(
                name: "XuanxiangB",
                table: "ZVote");

            migrationBuilder.DropColumn(
                name: "XuanxiangC",
                table: "ZVote");

            migrationBuilder.DropColumn(
                name: "XuanxiangD",
                table: "ZVote");

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "ZVote",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxItemCountpu",
                table: "ZVote",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
