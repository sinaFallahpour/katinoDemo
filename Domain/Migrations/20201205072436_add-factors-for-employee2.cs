using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addfactorsforemployee2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeePlanId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeePlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdataAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFactors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true),
                    EmployeePlanId = table.Column<int>(nullable: true),
                    TrackingCode = table.Column<string>(nullable: true),
                    IsBackMOney = table.Column<bool>(nullable: false),
                    PaymnetType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFactors_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeFactors_EmployeePlans_EmployeePlanId",
                        column: x => x.EmployeePlanId,
                        principalTable: "EmployeePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(nullable: false),
                    IsSucceed = table.Column<bool>(nullable: false),
                    InvoiceKey = table.Column<string>(nullable: true),
                    TransactionCode = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    TrackingNumber = table.Column<string>(nullable: true),
                    ErrorDescription = table.Column<string>(nullable: true),
                    ErrorCode = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<string>(nullable: true),
                    EmployeePlanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePayments_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePayments_EmployeePlans_EmployeePlanId",
                        column: x => x.EmployeePlanId,
                        principalTable: "EmployeePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionCode = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Information = table.Column<string>(nullable: true),
                    EmployeePaymentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeTransactions_EmployeePayments_EmployeePaymentId",
                        column: x => x.EmployeePaymentId,
                        principalTable: "EmployeePayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeePlanId",
                table: "AspNetUsers",
                column: "EmployeePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFactors_EmployeeId",
                table: "EmployeeFactors",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFactors_EmployeePlanId",
                table: "EmployeeFactors",
                column: "EmployeePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayments_EmployeeId",
                table: "EmployeePayments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayments_EmployeePlanId",
                table: "EmployeePayments",
                column: "EmployeePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTransactions_EmployeePaymentId",
                table: "EmployeeTransactions",
                column: "EmployeePaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmployeePlans_EmployeePlanId",
                table: "AspNetUsers",
                column: "EmployeePlanId",
                principalTable: "EmployeePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployeePlans_EmployeePlanId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EmployeeFactors");

            migrationBuilder.DropTable(
                name: "EmployeeTransactions");

            migrationBuilder.DropTable(
                name: "EmployeePayments");

            migrationBuilder.DropTable(
                name: "EmployeePlans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeePlanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeePlanId",
                table: "AspNetUsers");
        }
    }
}
