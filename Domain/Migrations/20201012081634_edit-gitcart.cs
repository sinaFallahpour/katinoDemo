using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editgitcart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts",
                column: "GiftCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCarts",
                column: "GiftCode",
                unique: true);
        }
    }
}
