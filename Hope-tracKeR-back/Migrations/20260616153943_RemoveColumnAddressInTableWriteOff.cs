using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hope_tracKeR_back.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumnAddressInTableWriteOff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WriteOffs_Addresses_AddressId",
                table: "WriteOffs");

            migrationBuilder.DropIndex(
                name: "IX_WriteOffs_AddressId",
                table: "WriteOffs");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "WriteOffs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "WriteOffs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WriteOffs_AddressId",
                table: "WriteOffs",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_WriteOffs_Addresses_AddressId",
                table: "WriteOffs",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
