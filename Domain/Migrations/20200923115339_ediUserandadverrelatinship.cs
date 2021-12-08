using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class ediUserandadverrelatinship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            
            

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobAdvertId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobAdvertisementId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobAdvertisementId",
                table: "AspNetUsers",
                column: "JobAdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobAdvertisements_JobAdvertisementId",
                table: "AspNetUsers",
                column: "JobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
