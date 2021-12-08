using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editfactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Plans_PlanId",
                table: "Factors");

            migrationBuilder.DropTable(
                name: "EmployeeAdver");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Factors",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "Factors",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Factors",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsImmediately",
                table: "Factors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Plans_PlanId",
                table: "Factors",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Plans_PlanId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "IsImmediately",
                table: "Factors");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "Factors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "Factors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discount",
                table: "Factors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.CreateTable(
                name: "EmployeeAdver",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    JobAdvertisementId = table.Column<int>(type: "int", nullable: false),
                    ResomeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAdver", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAdver_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeAdver_JobAdvertisements_JobAdvertisementId",
                        column: x => x.JobAdvertisementId,
                        principalTable: "JobAdvertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdver_EmployeeId",
                table: "EmployeeAdver",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdver_JobAdvertisementId",
                table: "EmployeeAdver",
                column: "JobAdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Plans_PlanId",
                table: "Factors",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
