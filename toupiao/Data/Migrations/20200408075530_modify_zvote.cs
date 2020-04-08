using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class modify_zvote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaveOnly",
                table: "ZVote");

            migrationBuilder.AddColumn<string>(
                name: "CoverPath",
                table: "ZVote",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPath",
                table: "ZVote");

            migrationBuilder.AddColumn<bool>(
                name: "IsSaveOnly",
                table: "ZVote",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
