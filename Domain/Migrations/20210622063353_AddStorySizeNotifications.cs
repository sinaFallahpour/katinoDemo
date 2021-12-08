using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddStorySizeNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StorySizeNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobAdvertisementId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorySizeNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorySizeNotifications_AspNetUsers_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorySizeNotifications_JobAdvertisements_JobAdvertisementId",
                        column: x => x.JobAdvertisementId,
                        principalTable: "JobAdvertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorySizeNotifications_CompanyId",
                table: "StorySizeNotifications",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StorySizeNotifications_JobAdvertisementId",
                table: "StorySizeNotifications",
                column: "JobAdvertisementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorySizeNotifications");
        }
    }
}
