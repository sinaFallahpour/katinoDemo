using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddResomeColorsToAdver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResomeColorId",
                table: "JobAdvertisements",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisements_ResomeColors_ResomeColorId",
                table: "JobAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_JobAdvertisements_ResomeColorId",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "ResomeColorId",
                table: "JobAdvertisements");
        }
    }
}
