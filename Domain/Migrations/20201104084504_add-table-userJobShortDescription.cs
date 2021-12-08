using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addtableuserJobShortDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserJobShortDescription",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    JobTitle = table.Column<string>(nullable: true),
                    LastCompanies = table.Column<string>(nullable: true),
                    LastEducationBackgrounds = table.Column<string>(nullable: true),
                    ResomeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobShortDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserJobShortDescription_Resomes_ResomeId",
                        column: x => x.ResomeId,
                        principalTable: "Resomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserJobShortDescription_ResomeId",
                table: "UserJobShortDescription",
                column: "ResomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserJobShortDescription");
        }
    }
}
