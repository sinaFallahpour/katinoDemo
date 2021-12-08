using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class edtadvrrr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilitaryType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MilitaryType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobAdvertisementId = table.Column<int>(type: "int", nullable: true),
                    Military = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryType_JobAdvertisements_JobAdvertisementId",
                        column: x => x.JobAdvertisementId,
                        principalTable: "JobAdvertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryType_JobAdvertisementId",
                table: "MilitaryType",
                column: "JobAdvertisementId");
        }
    }
}
