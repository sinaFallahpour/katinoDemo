using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class adingStaticNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telegram",
                table: "JobAdvertisements");

            migrationBuilder.AddColumn<string>(
                name: "StaticNumber",
                table: "JobAdvertisements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkTime",
                table: "JobAdvertisements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaticNumber",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "WorkTime",
                table: "JobAdvertisements");

            migrationBuilder.AddColumn<string>(
                name: "Telegram",
                table: "JobAdvertisements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
