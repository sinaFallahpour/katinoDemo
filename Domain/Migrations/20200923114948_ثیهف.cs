using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class ثیهف : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobAdvertisements_JobAdvertisementId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobAdvertId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "JobAdvertisementId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobAdvertisements_JobAdvertisementId",
                table: "AspNetUsers",
                column: "JobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
