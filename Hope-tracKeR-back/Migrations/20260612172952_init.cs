using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hope_tracKeR_back.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Diagnosis = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repairs_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Repairs_Items_ItemId",
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
                table: "Users",
                columns: new[] { "Id", "FullName", "Login", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "Рустам Вордович", "word365", "admin", 0 },
                    { 2, "Родион Экселович", "excel2003", "admin", 0 },
                    { 3, "Моисей Шарпович", "sharp3000", "admin", 0 },
                    { 4, "Михаил Реактович", "react2026", "admin", 0 },
                    { 5, "Кирилл TracKeR", "legenda212", "hope", 1 }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AddedDate", "AddressId", "BrandId", "Category", "Name", "SerialId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 0, "Монитор Samsung Odyssey", "SAMS-OD-001", 0 },
                    { 2, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, 3, 0, "Ноутбук LG Gram", "LG-GRAM-002", 2 },
                    { 3, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, 0, "Клавиатура Logitech MX", "LOG-MX-003", 0 },
                    { 4, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, 2, 1, "Бумага A4 500л", "PAP-A4-004", 0 },
                    { 5, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, 1, "Картридж для принтера", "CRTG-005", 0 },
                    { 6, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 1, "USB Flash Drive 32GB", "USB-32-006", 1 }
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

            migrationBuilder.InsertData(
                table: "Repairs",
                columns: new[] { "Id", "AddressId", "Description", "Diagnosis", "EndDate", "ItemId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Монитор не включается, индикатор питания не горит", "Неисправен блок питания, замена конденсаторов", new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, 2, "Ноутбук зависает при загрузке Windows", "Ожидается диагностика", null, 2, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 3, 1, "Клавиатура не печатает буквы, некоторые кнопки залипли", "Механическое повреждение, требуется чистка", null, 3, new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 4, 1, "Флешка не определяется компьютером", "Сбой контроллера, данные восстановлены", new DateTime(2025, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), 6, new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5, 1, "Монитор моргает и периодически гаснет", "Неисправен шлейф матрицы", null, 1, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 6, 2, "Ноутбук сильно греется и выключается", "Ожидается диагностика", null, 2, new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), 0 }
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

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_AddressId",
                table: "Repairs",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_ItemId",
                table: "Repairs",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemAttributes");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
