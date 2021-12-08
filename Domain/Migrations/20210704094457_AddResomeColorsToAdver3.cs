using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddResomeColorsToAdver3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResomeColorBg",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "ResomeColorText",
                table: "JobAdvertisements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResomeColorBg",
                table: "JobAdvertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResomeColorText",
                table: "JobAdvertisements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
