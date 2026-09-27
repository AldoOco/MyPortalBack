using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPortalBack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenFamilyUuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TokenFamilyUuid",
                table: "RefreshTokens",
                type: "uuid",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenFamilyUuid",
                table: "RefreshTokens",
                column: "TokenFamilyUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_TokenFamilyUuid",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "TokenFamilyUuid",
                table: "RefreshTokens");
        }
    }
}
