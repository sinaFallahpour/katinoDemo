using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addtableLikeAsignResome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikeAsignResomes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    AsignResomeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeAsignResomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikeAsignResomes_AsignResomes_AsignResomeId",
                        column: x => x.AsignResomeId,
                        principalTable: "AsignResomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikeAsignResomes_AsignResomeId",
                table: "LikeAsignResomes",
                column: "AsignResomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeAsignResomes");
        }
    }
}
