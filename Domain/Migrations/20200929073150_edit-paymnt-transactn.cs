using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editpaymnttransactn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

          
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Transactions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Payments",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PlanId",
                table: "Transactions",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Plans_PlanId",
                table: "Transactions",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
