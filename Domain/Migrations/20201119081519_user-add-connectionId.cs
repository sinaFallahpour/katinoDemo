using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class useraddconnectionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConecctionId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConecctionId",
                table: "AspNetUsers");
        }
    }
}
