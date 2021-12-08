using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class edituserandadver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "VisitThisAdver",
                table: "JobAdvertisements",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VisitThisAdverInSite",
                table: "JobAdvertisements",
                nullable: false,
                defaultValue: 0L);


          

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobAdvertisements_JobAdvertisementId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobAdvertisementId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisitThisAdver",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "VisitThisAdverInSite",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "JobAdvertisementId",
                table: "AspNetUsers");
        }
    }
}
