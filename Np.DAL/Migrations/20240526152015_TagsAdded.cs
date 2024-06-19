using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Np.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TagsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tag");
        }
    }
}
