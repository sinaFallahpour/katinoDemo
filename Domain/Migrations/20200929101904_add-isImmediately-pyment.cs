using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addisImmediatelypyment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Plans_PlanId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "Payments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsImmeditely",
                table: "Payments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Plans_PlanId",
                table: "Payments",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Plans_PlanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsImmeditely",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Plans_PlanId",
                table: "Payments",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
