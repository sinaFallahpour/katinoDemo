using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class edittableadvernotifadduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AdvertismentNotifications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertismentNotifications_UserId",
                table: "AdvertismentNotifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertismentNotifications_AspNetUsers_UserId",
                table: "AdvertismentNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertismentNotifications_AspNetUsers_UserId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdvertismentNotifications_UserId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AdvertismentNotifications");
        }
    }
}
