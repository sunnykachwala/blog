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
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.ActivityLogId);
                });

            migrationBuilder.CreateTable(
                name: "AdminUserRole",
                columns: table => new
                {
                    UserRoleGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDefaultRole = table.Column<bool>(type: "bit", nullable: false),
                    DefaultHome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserRole", x => x.UserRoleGuid);
                });

            migrationBuilder.CreateTable(
                name: "AppSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ApplicationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AppIcon = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DefaultCustomer = table.Column<int>(type: "int", nullable: false),
                    SaleAccount = table.Column<int>(type: "int", nullable: false),
                    PurchaseAccount = table.Column<int>(type: "int", nullable: false),
                    PayrollAccount = table.Column<int>(type: "int", nullable: false),
                    Copyright = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyCity = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyState = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyPostalCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyCountry = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyTaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultTimezone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DefaultLanguage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DefaultCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MailProtocol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MailEncryption = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MailHost = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MailPort = table.Column<int>(type: "int", nullable: false),
                    MailUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MailPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SendInvoice = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceTemplate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThemeLayout = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ThemeAppBar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ThemeSideBar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AwardedPointsPerSpent = table.Column<int>(type: "int", nullable: false),
                    RewardPointsWorth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetting", x => x.Id);
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
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisation", x => x.OrganisationGuid);
                });

            migrationBuilder.CreateTable(
                name: "Poll",
                columns: table => new
                {
                    PollId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poll", x => x.PollId);
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
                name: "Subscriber",
                columns: table => new
                {
                    SubscriberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsSubscribed = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber", x => x.SubscriberId);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TagName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DispalyOrder = table.Column<int>(type: "int", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
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
                name: "UserPermission",
                columns: table => new
                {
                    UserPermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Permission = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.UserPermissionId);
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
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    AvatarUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AboutUser = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.UserGuid);
                    table.ForeignKey(
                        name: "FK_AdminUser_AdminUserRole_UserRoleGuid",
                        column: x => x.UserRoleGuid,
                        principalTable: "AdminUserRole",
                        principalColumn: "UserRoleGuid",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "PollQuestion",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    PollId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollQuestion", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_PollQuestion_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "PollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultImage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DispalyOrder = table.Column<int>(type: "int", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Article_AdminUser_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AdminUser",
                        principalColumn: "UserGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollAnswer",
                columns: table => new
                {
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollAnswer", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_PollAnswer_PollQuestion_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "PollQuestion",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleCategory",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategory", x => new { x.ArticleId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ArticleCategory_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleComment",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_ArticleComment_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTag_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleView",
                columns: table => new
                {
                    ArticleViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ViewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SystemUser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "suser_sname()"),
                    AppName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValueSql: "app_name()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleView", x => x.ArticleViewId);
                    table.ForeignKey(
                        name: "FK_ArticleView_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberAnswer",
                columns: table => new
                {
                    SubscriberAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PollAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberAnswer", x => x.SubscriberAnswerId);
                    table.ForeignKey(
                        name: "FK_SubscriberAnswer_PollAnswer_SubscriberAnswerId",
                        column: x => x.SubscriberAnswerId,
                        principalTable: "PollAnswer",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriberAnswer_Subscriber_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscriber",
                        principalColumn: "SubscriberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_UserEmail_OrganisationGuid",
                table: "AdminUser",
                columns: new[] { "UserEmail", "OrganisationGuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_UserRoleGuid",
                table: "AdminUser",
                column: "UserRoleGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_AuthorId",
                table: "Article",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_Slug",
                table: "Article",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategory_CategoryId",
                table: "ArticleCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComment_ArticleId",
                table: "ArticleComment",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagId",
                table: "ArticleTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleView_ArticleId",
                table: "ArticleView",
                column: "ArticleId");

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
                name: "IX_PollAnswer_QuestionId",
                table: "PollAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestion_PollId",
                table: "PollQuestion",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMapping_UserPermissionId_UserRoleId",
                table: "RolePermissionMapping",
                columns: new[] { "UserPermissionId", "UserRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMapping_UserRoleId",
                table: "RolePermissionMapping",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberAnswer_SubscriberId_SubscriberAnswerId",
                table: "SubscriberAnswer",
                columns: new[] { "SubscriberId", "SubscriberAnswerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Slug",
                table: "Tag",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_TagName",
                table: "Tag",
                column: "TagName",
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
                name: "AppSetting");

            migrationBuilder.DropTable(
                name: "ArticleCategory");

            migrationBuilder.DropTable(
                name: "ArticleComment");

            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropTable(
                name: "ArticleView");

            migrationBuilder.DropTable(
                name: "AuditRecords");

            migrationBuilder.DropTable(
                name: "LoginResetHistory");

            migrationBuilder.DropTable(
                name: "Organisation");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RolePermissionMapping");

            migrationBuilder.DropTable(
                name: "SubscriberAnswer");

            migrationBuilder.DropTable(
                name: "UrlRecord");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "PollAnswer");

            migrationBuilder.DropTable(
                name: "Subscriber");

            migrationBuilder.DropTable(
                name: "AdminUser");

            migrationBuilder.DropTable(
                name: "PollQuestion");

            migrationBuilder.DropTable(
                name: "AdminUserRole");

            migrationBuilder.DropTable(
                name: "Poll");
        }
    }
}
