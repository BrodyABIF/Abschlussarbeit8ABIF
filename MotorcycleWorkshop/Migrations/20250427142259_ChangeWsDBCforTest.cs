using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleWorkshop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWsDBCforTest : Migration
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

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "AlternateId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, new Guid("795c9e8c-8613-45c7-bdf9-a4942635f09e"), "Oil Filter", 20.99m },
                    { 2, new Guid("cfe15534-d283-4d36-82fe-67b63acb6974"), "Brake Pads", 45.50m }
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Email", "Name", "PersonType", "PhoneNumber", "CustomerCity", "CustomerPostalCode", "CustomerStreet" },
                values: new object[] { 1, new Guid("4da883cb-139b-4487-a717-2c7a7e63114f"), "customer@mail.at", "Customer Horst", "Customer", "012345", "Vienna", "1010", "Customerstrasse 1" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "AlternateId", "Certification", "Email", "HourlyRate", "Name", "PersonType", "PhoneNumber", "MechanicCity", "MechanicPostalCode", "MechanicStreet" },
                values: new object[] { 2, new Guid("1e4bbdde-306e-487f-8a67-f19434236ab0"), "Certified Mechanic", "jane.smith@example.com", 50.0m, "Jane Smith", "Mechanic", "012345", "Vienna", "1020", "Repair St. 456" });

            migrationBuilder.InsertData(
                table: "Motorcycles",
                columns: new[] { "Id", "AlternateId", "Mileage", "Model", "OwnerId", "Year" },
                values: new object[] { 1, new Guid("2956e0cc-466e-4ad7-af2c-4572d379b600"), 5000.00m, "Honda CBR600RR", 1, 2020 });

            migrationBuilder.InsertData(
                table: "Repairs",
                columns: new[] { "Id", "AlternateId", "CustomerId", "MechanicId", "RepairDate" },
                values: new object[] { 1, new Guid("39a3e988-8a0c-47df-8eca-93ad92988ec4"), 1, 2, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
