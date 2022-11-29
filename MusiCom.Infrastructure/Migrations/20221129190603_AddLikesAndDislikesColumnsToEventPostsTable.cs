using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusiCom.Infrastructure.Migrations
{
    public partial class AddLikesAndDislikesColumnsToEventPostsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfDislikes",
                table: "EventPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLikes",
                table: "EventPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfDislikes",
                table: "EventPosts");

            migrationBuilder.DropColumn(
                name: "NumberOfLikes",
                table: "EventPosts");
        }
    }
}
