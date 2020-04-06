using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class Add_ZVote_Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ZVote",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ZVote");
        }
    }
}
