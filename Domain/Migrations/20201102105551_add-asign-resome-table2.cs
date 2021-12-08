using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addasignresometable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KatinoPDFResome",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PDFResome",
                table: "Resomes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AsignResomes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    AsingResomeStatus = table.Column<int>(nullable: false),
                    EmployerDescriptioin = table.Column<string>(nullable: true),
                    ResomeId = table.Column<int>(nullable: false),
                    JobAdvertisementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignResomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignResomes_JobAdvertisements_JobAdvertisementId",
                        column: x => x.JobAdvertisementId,
                        principalTable: "JobAdvertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignResomes_Resomes_ResomeId",
                        column: x => x.ResomeId,
                        principalTable: "Resomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignResomes_JobAdvertisementId",
                table: "AsignResomes",
                column: "JobAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AsignResomes_ResomeId",
                table: "AsignResomes",
                column: "ResomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsignResomes");

            migrationBuilder.DropColumn(
                name: "KatinoPDFResome",
                table: "Resomes");

            migrationBuilder.DropColumn(
                name: "PDFResome",
                table: "Resomes");
        }
    }
}
