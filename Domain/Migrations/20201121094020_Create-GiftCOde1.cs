using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class CreateGiftCOde1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCarts_AspNetUsers_EmployerId",
                table: "GiftCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftCarts",
                table: "GiftCarts");

            migrationBuilder.RenameTable(
                name: "GiftCarts",
                newName: "GiftCart");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCarts_GiftCode",
                table: "GiftCart",
                newName: "IX_GiftCart_GiftCode");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCarts_EmployerId",
                table: "GiftCart",
                newName: "IX_GiftCart_EmployerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftCart",
                table: "GiftCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCart_AspNetUsers_EmployerId",
                table: "GiftCart",
                column: "EmployerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
