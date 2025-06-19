using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleWorkshop.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Repairs_RepairId",
                table: "Invoices");

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

            migrationBuilder.AlterColumn<int>(
                name: "RepairId",
                table: "Invoices",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "AlternateId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, new Guid("6fc80d59-4ac6-4872-bfbb-536d975c606f"), "Oil Filter", 20.99m },
                    { 2, new Guid("0233245d-85cc-480b-aee1-ac6a330aadb8"), "Brake Pads", 45.50m }
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Email", "Name", "PersonType", "PhoneNumber", "CustomerCity", "CustomerPostalCode", "CustomerStreet" },
                values: new object[] { 1, new Guid("22b6b861-5520-479e-afb4-d4fe3b7a78fe"), "customer@mail.at", "Customer Horst", "Customer", "012345", "Vienna", "1010", "Customerstrasse 1" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Certification", "Email", "HourlyRate", "Name", "PersonType", "PhoneNumber", "MechanicCity", "MechanicPostalCode", "MechanicStreet" },
                values: new object[] { 2, new Guid("eebe378d-83ca-4524-98be-64d5bdc524c5"), "Certified Mechanic", "jane.smith@example.com", 50.0m, "Jane Smith", "Mechanic", "012345", "Vienna", "1020", "Repair St. 456" });

            migrationBuilder.InsertData(
                table: "Motorcycles",
                columns: new[] { "Id", "AlternateId", "Mileage", "Model", "OwnerId", "Year" },
                values: new object[] { 1, new Guid("24f2ce5e-9832-41ca-8f4d-6b214cc46916"), 5000.00m, "Honda CBR600RR", 1, 2020 });

            migrationBuilder.InsertData(
                table: "Repairs",
                columns: new[] { "Id", "AlternateId", "CustomerId", "MechanicId", "RepairDate" },
                values: new object[] { 1, new Guid("ba994b95-8cde-4da2-9198-9df8e3dca064"), 1, 2, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Repairs_RepairId",
                table: "Invoices",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Repairs_RepairId",
                table: "Invoices");

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

            migrationBuilder.AlterColumn<int>(
                name: "RepairId",
                table: "Invoices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Repairs_RepairId",
                table: "Invoices",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
