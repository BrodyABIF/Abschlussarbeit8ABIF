using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleWorkshop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlternateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.UniqueConstraint("AK_Parts_AlternateId", x => x.AlternateId);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlternateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PersonType = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    CustomerStreet = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerCity = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerPostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    Certification = table.Column<string>(type: "TEXT", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    MechanicStreet = table.Column<string>(type: "TEXT", nullable: true),
                    MechanicCity = table.Column<string>(type: "TEXT", nullable: true),
                    MechanicPostalCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.UniqueConstraint("AK_Person_AlternateId", x => x.AlternateId);
                });

            migrationBuilder.CreateTable(
                name: "Motorcycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlternateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Mileage = table.Column<decimal>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycles", x => x.Id);
                    table.UniqueConstraint("AK_Motorcycles_AlternateId", x => x.AlternateId);
                    table.ForeignKey(
                        name: "FK_Motorcycles_Person_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlternateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    MechanicId = table.Column<int>(type: "INTEGER", nullable: false),
                    RepairDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                    table.UniqueConstraint("AK_Repairs_AlternateId", x => x.AlternateId);
                    table.ForeignKey(
                        name: "FK_Repairs_Person_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Repairs_Person_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairParts",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "INTEGER", nullable: false),
                    RepairId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairParts", x => new { x.PartId, x.RepairId });
                    table.ForeignKey(
                        name: "FK_RepairParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairParts_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "AlternateId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, new Guid("d65f8ba4-8b7f-418a-87a1-e900dbb62aec"), "Oil Filter", 20.99m },
                    { 2, new Guid("83d77397-f080-4baa-8109-e59d77295a99"), "Brake Pads", 45.50m }
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Email", "Name", "PersonType", "PhoneNumber", "CustomerCity", "CustomerPostalCode", "CustomerStreet" },
                values: new object[] { 1, new Guid("52b24478-988d-4bee-986a-d503cae9203f"), "customer@mail.at", "Customer Horst", "Customer", "012345", "Vienna", "1010", "Customerstrasse 1" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Certification", "Email", "HourlyRate", "Name", "PersonType", "PhoneNumber", "MechanicCity", "MechanicPostalCode", "MechanicStreet" },
                values: new object[] { 2, new Guid("bcd869d9-324a-4f96-a6fc-c404e8769630"), "Certified Mechanic", "jane.smith@example.com", 50.0m, "Jane Smith", "Mechanic", "012345", "Vienna", "1020", "Repair St. 456" });

            migrationBuilder.InsertData(
                table: "Motorcycles",
                columns: new[] { "Id", "AlternateId", "Mileage", "Model", "OwnerId", "Year" },
                values: new object[] { 1, new Guid("9bf890f1-ecc5-4ce8-a78a-26a920dca0fb"), 5000.00m, "Honda CBR600RR", 1, 2020 });

            migrationBuilder.InsertData(
                table: "Repairs",
                columns: new[] { "Id", "AlternateId", "CustomerId", "MechanicId", "RepairDate" },
                values: new object[] { 1, new Guid("44fe95ff-3dd2-4001-89aa-87582276b447"), 1, 2, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "RepairParts",
                columns: new[] { "PartId", "RepairId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_OwnerId",
                table: "Motorcycles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairParts_RepairId",
                table: "RepairParts",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_CustomerId",
                table: "Repairs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_MechanicId",
                table: "Repairs",
                column: "MechanicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Motorcycles");

            migrationBuilder.DropTable(
                name: "RepairParts");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
