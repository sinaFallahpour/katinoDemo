using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class adlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Factors_TransactionStatus",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_CompanyId_IsImmediately_TransactionStatus",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_CompanyId_PlanId_TransactionStatus",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "TransactionStatus",
                table: "Factors");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CompanyId_IsImmediately",
                table: "Factors",
                columns: new[] { "CompanyId", "IsImmediately" });

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CompanyId_PlanId",
                table: "Factors",
                columns: new[] { "CompanyId", "PlanId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Factors_CompanyId_IsImmediately",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_CompanyId_PlanId",
                table: "Factors");

            migrationBuilder.AddColumn<int>(
                name: "TransactionStatus",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Factors_TransactionStatus",
                table: "Factors",
                column: "TransactionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CompanyId_IsImmediately_TransactionStatus",
                table: "Factors",
                columns: new[] { "CompanyId", "IsImmediately", "TransactionStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Factors_CompanyId_PlanId_TransactionStatus",
                table: "Factors",
                columns: new[] { "CompanyId", "PlanId", "TransactionStatus" });
        }
    }
}
