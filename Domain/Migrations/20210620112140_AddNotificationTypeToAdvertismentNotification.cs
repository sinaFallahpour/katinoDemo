using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddNotificationTypeToAdvertismentNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertismentNotifications_JobAdvertisements_JobAdvertisementId",
                table: "AdvertismentNotifications");

            migrationBuilder.AlterColumn<int>(
                name: "JobAdvertisementId",
                table: "AdvertismentNotifications",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "AdvertismentNotifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "AdvertismentNotifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AdvertismentNotifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertismentNotifications_EmployeeId",
                table: "AdvertismentNotifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertismentNotifications_ResomeId",
                table: "AdvertismentNotifications",
                column: "ResomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertismentNotifications_AspNetUsers_EmployeeId",
                table: "AdvertismentNotifications",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertismentNotifications_JobAdvertisements_JobAdvertisementId",
                table: "AdvertismentNotifications",
                column: "JobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertismentNotifications_Resomes_ResomeId",
                table: "AdvertismentNotifications",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertismentNotifications_AspNetUsers_EmployeeId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertismentNotifications_JobAdvertisements_JobAdvertisementId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertismentNotifications_Resomes_ResomeId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdvertismentNotifications_EmployeeId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdvertismentNotifications_ResomeId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "AdvertismentNotifications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AdvertismentNotifications");

            migrationBuilder.AlterColumn<int>(
                name: "JobAdvertisementId",
                table: "AdvertismentNotifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertismentNotifications_JobAdvertisements_JobAdvertisementId",
                table: "AdvertismentNotifications",
                column: "JobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
