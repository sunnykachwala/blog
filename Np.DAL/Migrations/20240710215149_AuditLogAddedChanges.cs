using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Np.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AuditLogAddedChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    AuditLogGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ActivityLogId = table.Column<int>(type: "int", nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.AuditLogGuid);
                    table.ForeignKey(
                        name: "FK_AuditLog_ActivityLog_ActivityLogId",
                        column: x => x.ActivityLogId,
                        principalTable: "ActivityLog",
                        principalColumn: "ActivityLogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ActivityLogId",
                table: "AuditLog",
                column: "ActivityLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");
        }
    }
}
