using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editgiftcart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UseAt",
                table: "GiftCarts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts",
                column: "GiftCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UseAt",
                table: "GiftCarts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts",
                column: "GiftCode");
        }
    }
}
