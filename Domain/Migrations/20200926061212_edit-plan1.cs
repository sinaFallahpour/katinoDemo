using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editplan1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUsers_Plan_PlanId",
            //    table: "AspNetUsers");

            //migrationBuilder.DropTable(
            //    name: "Plan");

            //migrationBuilder.DropIndex(
            //    name: "IX_JobAdvertisements_ExpireTime",
            //    table: "JobAdvertisements");

            //migrationBuilder.DropIndex(
            //    name: "IX_AspNetUsers_PlanId",
            //    table: "AspNetUsers");

            //migrationBuilder.DropColumn(
            //    name: "PlanId",
            //    table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdverCount = table.Column<int>(type: "int", nullable: false),
                    AdverExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImmediateAdverCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsUseResomeManegement = table.Column<bool>(type: "bit", nullable: false),
                    Logo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdataAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
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
                name: "IX_Plan_AdverExpireTime",
                table: "Plan",
                column: "AdverExpireTime");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_Title",
                table: "Plan",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Plan_PlanId",
                table: "AspNetUsers",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
