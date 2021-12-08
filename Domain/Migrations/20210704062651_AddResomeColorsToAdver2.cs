using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddResomeColorsToAdver2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisements_ResomeColors_ResomeColorId",
                table: "JobAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_JobAdvertisements_ResomeColorId",
                table: "JobAdvertisements");

            migrationBuilder.AddColumn<string>(
                name: "ResomeColorBg",
                table: "JobAdvertisements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResomeColorText",
                table: "JobAdvertisements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResomeColorBg",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "ResomeColorText",
                table: "JobAdvertisements");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_ResomeColorId",
                table: "JobAdvertisements",
                column: "ResomeColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisements_ResomeColors_ResomeColorId",
                table: "JobAdvertisements",
                column: "ResomeColorId",
                principalTable: "ResomeColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
