using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addmulticolumnindexinfactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Factors_CompanyId",
                table: "Factors");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CompanyId_IsImmediately_TransactionStatus",
                table: "Factors",
                columns: new[] { "CompanyId", "IsImmediately", "TransactionStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CompanyId_PlanId_TransactionStatus",
                table: "Factors",
                columns: new[] { "CompanyId", "PlanId", "TransactionStatus" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Factors_CompanyId_IsImmediately_TransactionStatus",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_CompanyId_PlanId_TransactionStatus",
                table: "Factors");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CompanyId",
                table: "Factors",
                column: "CompanyId");
        }
    }
}
