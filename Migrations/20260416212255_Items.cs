using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Multi_Tenant_E_Commerce_API.Migrations
{
    /// <inheritdoc />
    public partial class Items : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_StoreAdminId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_SuperAdminId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_StoreAdminId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StoreAdminId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "SuperAdminId",
                table: "Items",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_SuperAdminId",
                table: "Items",
                newName: "IX_Items_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_UserId",
                table: "Items",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_UserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Items",
                newName: "SuperAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_UserId",
                table: "Items",
                newName: "IX_Items_SuperAdminId");

            migrationBuilder.AddColumn<int>(
                name: "StoreAdminId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_StoreAdminId",
                table: "Items",
                column: "StoreAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_StoreAdminId",
                table: "Items",
                column: "StoreAdminId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_SuperAdminId",
                table: "Items",
                column: "SuperAdminId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
