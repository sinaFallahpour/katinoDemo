using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addAdver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobAdvertisements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    TypeOfCooperation = table.Column<int>(nullable: false),
                    Salary = table.Column<int>(nullable: false),
                    WorkExperience = table.Column<int>(nullable: false),
                    DegreeOfEducation = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Military = table.Column<int>(nullable: false),
                    DescriptionOfJob = table.Column<string>(nullable: false),
                    DescriptionOfCompany = table.Column<string>(nullable: false),
                    AdverStatus = table.Column<int>(nullable: false),
                    ExpireTime = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAdvertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobAdvertisements_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_AdverStatus",
                table: "JobAdvertisements",
                column: "AdverStatus");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_CategoryId",
                table: "JobAdvertisements",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_City",
                table: "JobAdvertisements",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_Title",
                table: "JobAdvertisements",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_TypeOfCooperation",
                table: "JobAdvertisements",
                column: "TypeOfCooperation");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_Title_City",
                table: "JobAdvertisements",
                columns: new[] { "Title", "City" });

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_Title_City_TypeOfCooperation",
                table: "JobAdvertisements",
                columns: new[] { "Title", "City", "TypeOfCooperation" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobAdvertisements");
        }
    }
}
