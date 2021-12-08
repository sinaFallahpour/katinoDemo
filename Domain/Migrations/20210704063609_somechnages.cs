using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class somechnages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "JobAdvertisements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "JobAdvertisements");
        }
    }
}
