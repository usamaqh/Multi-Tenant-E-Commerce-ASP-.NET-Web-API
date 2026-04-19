using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Multi_Tenant_E_Commerce_API.Migrations
{
    /// <inheritdoc />
    public partial class namechangeduserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_SuperAdminId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SuperAdminId",
                table: "Users",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SuperAdminId",
                table: "Users",
                newName: "IX_Users_CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_SuperAdminId",
                table: "Companies",
                column: "SuperAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_SuperAdminId",
                table: "Companies",
                column: "SuperAdminId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedByUserId",
                table: "Users",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_SuperAdminId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedByUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Companies_SuperAdminId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Users",
                newName: "SuperAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CreatedByUserId",
                table: "Users",
                newName: "IX_Users_SuperAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_SuperAdminId",
                table: "Users",
                column: "SuperAdminId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
