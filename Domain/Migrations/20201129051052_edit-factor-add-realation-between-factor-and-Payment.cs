using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editfactoraddrealationbetweenfactorandPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Payments_PaymentId",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_PaymentId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Factors");
        }
    }
}
