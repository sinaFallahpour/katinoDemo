using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addingRelationBeetwennRefrencTransactionAndPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "RefrenceTransations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RefrenceTransations_PaymentId",
                table: "RefrenceTransations",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefrenceTransations_Payments_PaymentId",
                table: "RefrenceTransations",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefrenceTransations_Payments_PaymentId",
                table: "RefrenceTransations");

            migrationBuilder.DropIndex(
                name: "IX_RefrenceTransations_PaymentId",
                table: "RefrenceTransations");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "RefrenceTransations");
        }
    }
}
