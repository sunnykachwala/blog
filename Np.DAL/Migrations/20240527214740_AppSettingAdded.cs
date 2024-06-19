using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Np.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AppSettingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultHome",
                table: "AdminUserRole",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

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
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetting", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSetting");

            migrationBuilder.DropColumn(
                name: "DefaultHome",
                table: "AdminUserRole");
        }
    }
}
