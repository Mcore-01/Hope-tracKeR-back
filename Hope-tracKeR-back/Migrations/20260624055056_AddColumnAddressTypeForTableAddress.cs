using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hope_tracKeR_back.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnAddressTypeForTableAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressType",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddressType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddressType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddressType", "Branch", "Building", "Floor", "Room" },
                values: new object[] { 1, "ул. Пушкина 1", "Корпус 2", 2, "Кабинет 206" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "AddressType", "Branch", "Building", "Floor", "Room" },
                values: new object[,]
                {
                    { 5, 0, "ул. Толстого 31", "Корпус 4", 3, "Кабинет 314" },
                    { 6, 0, "ул. Толстого 31", "Корпус 4", 1, "Кабинет 114" },
                    { 7, 1, "ул. Толстого 31", "Корпус 1", 1, "Кабинет 102" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "Addresses");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Branch", "Building", "Floor", "Room" },
                values: new object[] { "ул. Толстого 31", "Корпус 4", 3, "Кабинет 314" });
        }
    }
}
