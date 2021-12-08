using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_TrackingNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Factors");

            migrationBuilder.AddColumn<decimal>(
                name: "FinallyAmountWithTax",
                table: "Transactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "Payments",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinallyAmountWithTax",
                table: "Payments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Date",
                table: "Payments",
                column: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_Date",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FinallyAmountWithTax",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FinallyAmountWithTax",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Factors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TrackingNumber",
                table: "Payments",
                column: "TrackingNumber",
                unique: true,
                filter: "[TrackingNumber] IS NOT NULL");
        }
    }
}
