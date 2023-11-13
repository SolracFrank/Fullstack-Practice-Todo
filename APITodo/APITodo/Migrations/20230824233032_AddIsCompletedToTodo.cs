using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APITodo.Migrations
{
    public partial class AddIsCompletedToTodo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "todos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "todos");
        }
    }
}
