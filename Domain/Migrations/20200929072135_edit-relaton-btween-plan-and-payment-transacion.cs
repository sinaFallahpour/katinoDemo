using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editrelatonbtweenplanandpaymenttransacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

           

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PlanId",
                table: "Payments",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Plans_PlanId",
                table: "Payments",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Plans_PlanId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Plans_PlanId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PlanId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PlanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Payments");
        }
    }
}
