using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Np.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    ActivityLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityLogName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LogType = table.Column<byte>(type: "tinyint", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.ActivityLogId);
                });

            migrationBuilder.CreateTable(
                name: "AdminUser",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserPasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashedConformationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConfirmedRegistration = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false),
                    EncryptedSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailOTP = table.Column<bool>(type: "bit", nullable: false),
                    UserRoleGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TwofactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    OrganisationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.UserGuid);
                });

            migrationBuilder.CreateTable(
                name: "AdminUserRole",
                columns: table => new
                {
                    UserRoleGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDefaultRole = table.Column<bool>(type: "bit", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserRole", x => x.UserRoleGuid);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CategoryImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DispalyOrder = table.Column<int>(type: "int", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoginResetHistory",
                columns: table => new
                {
                    LoginResetHistoryGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    HashedConfirmationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginResetHistory", x => x.LoginResetHistoryGuid);
                });

            migrationBuilder.CreateTable(
                name: "Organisation",
                columns: table => new
                {
                    OrganisationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrganisationName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_Organisation", x => x.OrganisationGuid);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Slug = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    EntityType = table.Column<byte>(type: "tinyint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditRecords",
                columns: table => new
                {
                    AuditRecordGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityLogId = table.Column<int>(type: "int", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRecords", x => x.AuditRecordGuid);
                    table.ForeignKey(
                        name: "FK_AuditRecords_ActivityLog_ActivityLogId",
                        column: x => x.ActivityLogId,
                        principalTable: "ActivityLog",
                        principalColumn: "ActivityLogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_UserEmail_OrganisationGuid",
                table: "AdminUser",
                columns: new[] { "UserEmail", "OrganisationGuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditRecords_ActivityLogId",
                table: "AuditRecords",
                column: "ActivityLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Slug",
                table: "Category",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Title",
                table: "Category",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organisation_OrganisationName",
                table: "Organisation",
                column: "OrganisationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlRecord_Slug",
                table: "UrlRecord",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUser");

            migrationBuilder.DropTable(
                name: "AdminUserRole");

            migrationBuilder.DropTable(
                name: "AuditRecords");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "LoginResetHistory");

            migrationBuilder.DropTable(
                name: "Organisation");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "UrlRecord");

            migrationBuilder.DropTable(
                name: "ActivityLog");
        }
    }
}
