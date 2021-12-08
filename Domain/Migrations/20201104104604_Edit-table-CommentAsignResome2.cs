using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class EdittableCommentAsignResome2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeAsignResomes_AsignResomes_AsignResomeId",
                table: "LikeAsignResomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikeAsignResomes",
                table: "LikeAsignResomes");

            migrationBuilder.RenameTable(
                name: "LikeAsignResomes",
                newName: "CommentAsignResomes");

            migrationBuilder.RenameIndex(
                name: "IX_LikeAsignResomes_AsignResomeId",
                table: "CommentAsignResomes",
                newName: "IX_CommentAsignResomes_AsignResomeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentAsignResomes",
                table: "CommentAsignResomes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAsignResomes_AsignResomes_AsignResomeId",
                table: "CommentAsignResomes",
                column: "AsignResomeId",
                principalTable: "AsignResomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAsignResomes_AsignResomes_AsignResomeId",
                table: "CommentAsignResomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentAsignResomes",
                table: "CommentAsignResomes");

            migrationBuilder.RenameTable(
                name: "CommentAsignResomes",
                newName: "LikeAsignResomes");

            migrationBuilder.RenameIndex(
                name: "IX_CommentAsignResomes_AsignResomeId",
                table: "LikeAsignResomes",
                newName: "IX_LikeAsignResomes_AsignResomeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikeAsignResomes",
                table: "LikeAsignResomes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeAsignResomes_AsignResomes_AsignResomeId",
                table: "LikeAsignResomes",
                column: "AsignResomeId",
                principalTable: "AsignResomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
