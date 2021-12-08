using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class somajska : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefrenceTransations_Payments_PaymentId",
                table: "RefrenceTransations");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "RefrenceTransations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RefrenceTransationId",
                table: "Payments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RefrenceTransationId",
                table: "Payments",
                column: "RefrenceTransationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RefrenceTransations_RefrenceTransationId",
                table: "Payments",
                column: "RefrenceTransationId",
                principalTable: "RefrenceTransations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefrenceTransations_Payments_PaymentId",
                table: "RefrenceTransations",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RefrenceTransations_RefrenceTransationId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_RefrenceTransations_Payments_PaymentId",
                table: "RefrenceTransations");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RefrenceTransationId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RefrenceTransationId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "RefrenceTransations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefrenceTransations_Payments_PaymentId",
                table: "RefrenceTransations",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
