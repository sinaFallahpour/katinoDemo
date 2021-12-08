using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class initplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Plan_PlanId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Plan_AdverExpireTime",
                table: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_Plan_Title",
                table: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_JobAdvertisements_ExpireTime",
                table: "JobAdvertisements");

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Plan",
                type: "float",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Plan_PlanId",
                table: "AspNetUsers",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
