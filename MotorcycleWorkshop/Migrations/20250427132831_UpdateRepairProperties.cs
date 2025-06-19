using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleWorkshop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRepairProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Motorcycles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Repairs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<string>(type: "TEXT", nullable: false),
                    RepairId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Repairs_RepairId",
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
                    { 1, new Guid("90ee982f-2944-42ec-b340-12f776e453b2"), "Oil Filter", 20.99m },
                    { 2, new Guid("7b79149d-8913-4198-b15e-be867011d741"), "Brake Pads", 45.50m }
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Email", "Name", "PersonType", "PhoneNumber", "CustomerCity", "CustomerPostalCode", "CustomerStreet" },
                values: new object[] { 1, new Guid("79d0925b-5d75-4985-957c-3757fe98db16"), "customer@mail.at", "Customer Horst", "Customer", "012345", "Vienna", "1010", "Customerstrasse 1" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Certification", "Email", "HourlyRate", "Name", "PersonType", "PhoneNumber", "MechanicCity", "MechanicPostalCode", "MechanicStreet" },
                values: new object[] { 2, new Guid("e69a287d-3206-4fc1-9012-378b3f1b4459"), "Certified Mechanic", "jane.smith@example.com", 50.0m, "Jane Smith", "Mechanic", "012345", "Vienna", "1020", "Repair St. 456" });

            migrationBuilder.InsertData(
                table: "Motorcycles",
                columns: new[] { "Id", "AlternateId", "Mileage", "Model", "OwnerId", "Year" },
                values: new object[] { 1, new Guid("34e64d7b-3ace-4680-909a-90db4773d847"), 5000.00m, "Honda CBR600RR", 1, 2020 });

            migrationBuilder.InsertData(
                table: "Repairs",
                columns: new[] { "Id", "AlternateId", "CustomerId", "MechanicId", "RepairDate" },
                values: new object[] { 1, new Guid("6059eba3-4e5b-4779-b737-db8f22e2ecf5"), 1, 2, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RepairId",
                table: "Invoices",
                column: "RepairId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DeleteData(
                table: "Motorcycles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Repairs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2);

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
        }
    }
}
