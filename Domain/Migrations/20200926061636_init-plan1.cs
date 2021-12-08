using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class initplan1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Discount = table.Column<double>(nullable: false,defaultValue:0),
                    Duration = table.Column<DateTime>(nullable: false),
                    Logo = table.Column<DateTime>(nullable: true),
                    AdverExpireTime = table.Column<DateTime>(nullable: false),
                    AdverCount = table.Column<int>(nullable: false),
                    ImmediateAdverCount = table.Column<int>(nullable: false),
                    IsUseResomeManegement = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdataAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_ExpireTime",
                table: "JobAdvertisements",
                column: "ExpireTime");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PlanId",
                table: "AspNetUsers",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_AdverExpireTime",
                table: "Plans",
                column: "AdverExpireTime");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Title",
                table: "Plans",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Plans_PlanId",
                table: "AspNetUsers",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Plans_PlanId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_JobAdvertisements_ExpireTime",
                table: "JobAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PlanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "AspNetUsers");
        }
    }
}
