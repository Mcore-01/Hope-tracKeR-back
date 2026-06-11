using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hope_tracKeR_back.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Branch = table.Column<string>(type: "text", nullable: false),
                    Building = table.Column<string>(type: "text", nullable: false),
                    Floor = table.Column<int>(type: "integer", nullable: false),
                    Room = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SerialId = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    BrandId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemAttributes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Branch", "Building", "Floor", "Room" },
                values: new object[,]
                {
                    { 1, "ул. Пушкина 1", "Корпус 1", 1, "Кабинет 101" },
                    { 2, "ул. Пушкина 1", "Корпус 1", 1, "Кабинет 102" },
                    { 3, "ул. Толстого 31", "Корпус 4", 3, "Кабинет 314" }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Samsung" },
                    { 2, "Dexp" },
                    { 3, "LG" },
                    { 4, "Logitech" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AddedDate", "AddressId", "BrandId", "Category", "Name", "SerialId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 14, 19, 0, 0, 0, DateTimeKind.Utc), 1, 1, 0, "Монитор Samsung Odyssey", "SAMS-OD-001", 0 },
                    { 2, new DateTime(2024, 2, 9, 19, 0, 0, 0, DateTimeKind.Utc), 2, 3, 0, "Ноутбук LG Gram", "LG-GRAM-002", 2 },
                    { 3, new DateTime(2024, 3, 4, 19, 0, 0, 0, DateTimeKind.Utc), 1, 4, 0, "Клавиатура Logitech MX", "LOG-MX-003", 0 },
                    { 4, new DateTime(2024, 3, 19, 19, 0, 0, 0, DateTimeKind.Utc), 3, 2, 1, "Бумага A4 500л", "PAP-A4-004", 0 },
                    { 5, new DateTime(2024, 3, 31, 19, 0, 0, 0, DateTimeKind.Utc), 2, 1, 1, "Картридж для принтера", "CRTG-005", 0 },
                    { 6, new DateTime(2024, 1, 24, 19, 0, 0, 0, DateTimeKind.Utc), 1, 2, 1, "USB Flash Drive 32GB", "USB-32-006", 1 }
                });

            migrationBuilder.InsertData(
                table: "ItemAttributes",
                columns: new[] { "Id", "ItemId", "Name", "Value" },
                values: new object[,]
                {
                    { 1, 1, "Диагональ", "27 дюмов" },
                    { 2, 1, "Разрешение", "2560x1440" },
                    { 3, 1, "Герцовка", "165Hz" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemAttributes_ItemId",
                table: "ItemAttributes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_AddressId",
                table: "Items",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId",
                table: "Items",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemAttributes");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
