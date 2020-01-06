using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment2.Migrations
{
    public partial class Professors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "professor",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "professorEmail",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "professor",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "professorEmail",
                table: "Courses");
        }
    }
}
