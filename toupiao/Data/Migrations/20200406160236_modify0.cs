using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class modify0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxItemCountpu",
                table: "ZVote",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxItemCountpu",
                table: "ZVote");
        }
    }
}
