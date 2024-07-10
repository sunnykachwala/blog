using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Np.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AuditLogChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "EntityType",
                table: "ActivityLog",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "ActivityLog",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryKeyValue",
                table: "ActivityLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "PrimaryKeyValue",
                table: "ActivityLog");
        }
    }
}
