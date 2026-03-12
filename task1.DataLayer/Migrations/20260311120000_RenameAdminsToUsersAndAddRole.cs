using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task1.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class RenameAdminsToUsersAndAddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "Admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Admins");
        }
    }
}
