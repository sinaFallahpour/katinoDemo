using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class EdittableCommentAsignResome3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "CommentAsignResomes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "CommentAsignResomes");
        }
    }
}
