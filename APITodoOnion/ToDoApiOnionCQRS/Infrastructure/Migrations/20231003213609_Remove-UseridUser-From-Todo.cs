using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUseridUserFromTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todo_users_UserIdUser",
                table: "Todo");

            migrationBuilder.DropIndex(
                name: "IX_Todo_UserIdUser",
                table: "Todo");

            migrationBuilder.DropColumn(
                name: "UserIdUser",
                table: "Todo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserIdUser",
                table: "Todo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todo_UserIdUser",
                table: "Todo",
                column: "UserIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_users_UserIdUser",
                table: "Todo",
                column: "UserIdUser",
                principalTable: "users",
                principalColumn: "id_user");
        }
    }
}
