using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addindexinfactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Factors_Date",
                table: "Factors",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_IsImmediately",
                table: "Factors",
                column: "IsImmediately");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_TransactionStatus",
                table: "Factors",
                column: "TransactionStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Factors_Date",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_IsImmediately",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_TransactionStatus",
                table: "Factors");
        }
    }
}
