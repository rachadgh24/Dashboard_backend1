using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

namespace task1.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class RenameUsersToCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Users_UserId",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Customers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Cars",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_UserId",
                table: "Cars",
                newName: "IX_Cars_CustomerId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Customers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Customers_CustomerId",
                table: "Cars",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Customers_CustomerId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Cars",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CustomerId",
                table: "Cars",
                newName: "IX_Cars_UserId");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Users_UserId",
                table: "Cars",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
