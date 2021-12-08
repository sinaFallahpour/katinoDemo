using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editadverr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Military",
                table: "JobAdvertisements");

            migrationBuilder.CreateTable(
                name: "MilitaryType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Military = table.Column<int>(nullable: false),
                    JobAdvertisementId = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilitaryType");

            migrationBuilder.AddColumn<int>(
                name: "Military",
                table: "JobAdvertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
