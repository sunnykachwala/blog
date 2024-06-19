using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Np.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangesRoleAndPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutUser",
                table: "AdminUser",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "AdminUser",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RolePermissionMapping",
                columns: table => new
                {
                    RolePermissionMappingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserPermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionMapping", x => x.RolePermissionMappingId);
                    table.ForeignKey(
                        name: "FK_RolePermissionMapping_AdminUserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "AdminUserRole",
                        principalColumn: "UserRoleGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    UserPermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Permission = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.UserPermissionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMapping_UserPermissionId_UserRoleId",
                table: "RolePermissionMapping",
                columns: new[] { "UserPermissionId", "UserRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMapping_UserRoleId",
                table: "RolePermissionMapping",
                column: "UserRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissionMapping");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropColumn(
                name: "AboutUser",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "AdminUser");
        }
    }
}
