using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class sas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "JobAdvertisements",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStorySaz",
                table: "JobAdvertisements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "JobAdvertisements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telegram",
                table: "JobAdvertisements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "IsStorySaz",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "Telegram",
                table: "JobAdvertisements");
        }
    }
}
