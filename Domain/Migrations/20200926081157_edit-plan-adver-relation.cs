using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editplanadverrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {



            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "JobAdvertisements",
                nullable: true);

           

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisements_Plans_PlanId",
                table: "JobAdvertisements",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisements_Plans_PlanId",
                table: "JobAdvertisements");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "JobAdvertisements",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisements_Plans_PlanId",
                table: "JobAdvertisements",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
