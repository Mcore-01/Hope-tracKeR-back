using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hope_tracKeR_back.Migrations
{
    /// <inheritdoc />
    public partial class EditColumnAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "NewValues",
                table: "AuditLogs",
                newName: "Values");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Values",
                table: "AuditLogs",
                newName: "NewValues");

            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "AuditLogs",
                type: "text",
                nullable: true);
        }
    }
}
