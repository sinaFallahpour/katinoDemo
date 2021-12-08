using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addUserJobSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserJobSkills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    JobSkillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserJobSkills_JobSkills_JobSkillId",
                        column: x => x.JobSkillId,
                        principalTable: "JobSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserJobSkills_CreateDate",
                table: "UserJobSkills",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobSkills_JobSkillId",
                table: "UserJobSkills",
                column: "JobSkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserJobSkills");
        }
    }
}
