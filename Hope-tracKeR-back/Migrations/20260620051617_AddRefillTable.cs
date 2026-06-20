using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hope_tracKeR_back.Migrations
{
    /// <inheritdoc />
    public partial class AddRefillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Refills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refills_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Refills_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Refills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Refills",
                columns: new[] { "Id", "AddressId", "EndDate", "ItemId", "StartDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 2, 14, 30, 0, 0, DateTimeKind.Utc), 101, new DateTime(2025, 6, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 2, 1, null, 103, new DateTime(2025, 6, 5, 9, 0, 0, 0, DateTimeKind.Utc), 1, 2 },
                    { 3, 1, null, 102, new DateTime(2025, 6, 10, 11, 15, 0, 0, DateTimeKind.Utc), 0, 3 },
                    { 4, 1, new DateTime(2025, 6, 13, 16, 0, 0, 0, DateTimeKind.Utc), 104, new DateTime(2025, 6, 12, 8, 30, 0, 0, DateTimeKind.Utc), 1, 4 },
                    { 5, 1, null, 105, new DateTime(2025, 6, 15, 14, 0, 0, 0, DateTimeKind.Utc), 1, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Refills_AddressId",
                table: "Refills",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Refills_ItemId",
                table: "Refills",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Refills_UserId",
                table: "Refills",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Refills");
        }
    }
}
