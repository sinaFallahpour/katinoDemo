using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addjobadvetismentrelationwithplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisements_Plans_PlanId",
                table: "JobAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_JobAdvertisements_PlanId",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "JobAdvertisements");
        }
    }
}
