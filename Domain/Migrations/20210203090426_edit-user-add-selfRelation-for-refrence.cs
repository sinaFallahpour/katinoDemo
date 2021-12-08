using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class edituseraddselfRelationforrefrence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefrenceId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RefrenceId",
                table: "AspNetUsers",
                column: "RefrenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RefrenceId",
                table: "AspNetUsers",
                column: "RefrenceId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RefrenceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RefrenceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefrenceId",
                table: "AspNetUsers");
        }
    }
}
