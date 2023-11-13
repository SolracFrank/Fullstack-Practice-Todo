using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APITodo.Migrations
{
    public partial class ConfigureFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todos_AspNetUsers_userId",
                table: "todos");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "todos",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_todos_userId",
                table: "todos",
                newName: "IX_todos_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_todos_AspNetUsers_UserId",
                table: "todos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todos_AspNetUsers_UserId",
                table: "todos");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "todos",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_todos_UserId",
                table: "todos",
                newName: "IX_todos_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_todos_AspNetUsers_userId",
                table: "todos",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
