using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editgiftCartRelation5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            //migrationBuilder.AddColumn<string>(
            //   name: "EmployerId",
            //   table: "GiftCarts",
            //   type: "nvarchar(450)",
            //   nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftCarts_EmployerId",
                table: "GiftCarts",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCarts_AspNetUsers_EmployerId",
                table: "GiftCarts",
                column: "EmployerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GiftCarts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftCarts_UserId",
                table: "GiftCarts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCarts_AspNetUsers_UserId",
                table: "GiftCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
