using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hope_tracKeR_back.Migrations
{
    /// <inheritdoc />
    public partial class AddCartridgeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cartridge_Status",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrinterModel",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AddressId", "BrandId", "Discriminator", "Name", "PrinterModel", "Cartridge_Status" },
                values: new object[,]
                {
                    { 100, 1, 1, "Cartridge", "Тонер-картридж Samsung ML-1660", "ML-1660", 0 },
                    { 101, 1, 1, "Cartridge", "Тонер-картридж Samsung SCX-4521F", "SCX-4521F", 2 },
                    { 102, 1, 3, "Cartridge", "Картридж LG Printronix", "Printronix", 3 },
                    { 103, 2, 2, "Cartridge", "Тонер Dexp DPP-250", "DPP-250", 1 },
                    { 104, 2, 1, "Cartridge", "Картридж Samsung CLP-315", "CLP-315", 1 },
                    { 105, 3, 3, "Cartridge", "Картридж LG LBP-1210", "LBP-1210", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DropColumn(
                name: "Cartridge_Status",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PrinterModel",
                table: "Items");
        }
    }
}
