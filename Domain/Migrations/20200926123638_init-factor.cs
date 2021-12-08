using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class initfactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Factor_AspNetUsers_CompanyId",
            //    table: "Factor");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Factor_Plans_PlanId",
            //    table: "Factor");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Factor",
            //    table: "Factor");

            //migrationBuilder.RenameTable(
            //    name: "Factor",
            //    newName: "Factors");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Factor_PlanId",
            //    table: "Factors",
            //    newName: "IX_Factors_PlanId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Factor_CompanyId",
            //    table: "Factors",
            //    newName: "IX_Factors_CompanyId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Factors",
            //    table: "Factors",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Factors_AspNetUsers_CompanyId",
            //    table: "Factors",
            //    column: "CompanyId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Factors_Plans_PlanId",
            //    table: "Factors",
            //    column: "PlanId",
            //    principalTable: "Plans",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_AspNetUsers_CompanyId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Plans_PlanId",
                table: "Factors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Factors",
                table: "Factors");

            migrationBuilder.RenameTable(
                name: "Factors",
                newName: "Factor");

            migrationBuilder.RenameIndex(
                name: "IX_Factors_PlanId",
                table: "Factor",
                newName: "IX_Factor_PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Factors_CompanyId",
                table: "Factor",
                newName: "IX_Factor_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Factor",
                table: "Factor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Factor_AspNetUsers_CompanyId",
                table: "Factor",
                column: "CompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factor_Plans_PlanId",
                table: "Factor",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
