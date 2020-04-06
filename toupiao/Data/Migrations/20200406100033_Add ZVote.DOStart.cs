using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toupiao.Data.Migrations
{
    public partial class AddZVoteDOStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DOStart",
                table: "ZVote",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOStart",
                table: "ZVote");
        }
    }
}
