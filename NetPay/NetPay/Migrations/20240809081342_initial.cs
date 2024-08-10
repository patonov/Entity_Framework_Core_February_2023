using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetPay.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    HouseholdId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuppliersServices",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliersServices", x => new { x.SupplierId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_SuppliersServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuppliersServices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "ServiceName" },
                values: new object[,]
                {
                    { 1, "Electricity" },
                    { 2, "Water" },
                    { 3, "Internet" },
                    { 4, "TV" },
                    { 5, "Phone" },
                    { 6, "Security" },
                    { 7, "Gas" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "SupplierName" },
                values: new object[,]
                {
                    { 1, "E-Service" },
                    { 2, "Light" },
                    { 3, "Energy-PRO" },
                    { 4, "ZEC" },
                    { 5, "Cellular" },
                    { 6, "A2one" },
                    { 7, "Telecom" },
                    { 8, "Cell2U" },
                    { 9, "DigiTV" },
                    { 10, "NetCom" },
                    { 11, "Net1" },
                    { 12, "MaxTel" },
                    { 13, "WaterSupplyCentral" },
                    { 14, "WaterSupplyNorth" },
                    { 15, "WaterSupplySouth" },
                    { 16, "FiberScreen" },
                    { 17, "SpeedNet" },
                    { 18, "GasGas" },
                    { 19, "BlueHome" },
                    { 20, "SecureHouse" },
                    { 21, "HomeGuard" },
                    { 22, "SafeHome" }
                });

            migrationBuilder.InsertData(
                table: "SuppliersServices",
                columns: new[] { "ServiceId", "SupplierId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 3, 5 },
                    { 4, 5 },
                    { 5, 5 },
                    { 3, 6 },
                    { 4, 6 },
                    { 5, 6 },
                    { 6, 6 },
                    { 3, 7 },
                    { 4, 7 },
                    { 5, 7 },
                    { 3, 8 },
                    { 4, 8 },
                    { 5, 8 },
                    { 3, 9 },
                    { 4, 9 },
                    { 3, 10 },
                    { 4, 10 },
                    { 6, 10 },
                    { 3, 11 },
                    { 4, 11 },
                    { 3, 12 },
                    { 4, 12 },
                    { 5, 12 },
                    { 6, 12 },
                    { 2, 13 },
                    { 2, 14 },
                    { 2, 15 },
                    { 3, 16 },
                    { 4, 16 },
                    { 3, 17 },
                    { 4, 17 },
                    { 6, 17 },
                    { 7, 18 },
                    { 7, 19 },
                    { 6, 20 },
                    { 6, 21 },
                    { 6, 22 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_HouseholdId",
                table: "Expenses",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ServiceId",
                table: "Expenses",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliersServices_ServiceId",
                table: "SuppliersServices",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "SuppliersServices");

            migrationBuilder.DropTable(
                name: "Households");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
