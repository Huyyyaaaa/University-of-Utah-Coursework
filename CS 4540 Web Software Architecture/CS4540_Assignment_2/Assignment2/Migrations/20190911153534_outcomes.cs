using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment2.Migrations
{
    public partial class outcomes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "LearningOutcomes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomes_CourseId",
                table: "LearningOutcomes",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningOutcomes_Courses_CourseId",
                table: "LearningOutcomes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcomes_Courses_CourseId",
                table: "LearningOutcomes");

            migrationBuilder.DropIndex(
                name: "IX_LearningOutcomes_CourseId",
                table: "LearningOutcomes");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "LearningOutcomes");
        }
    }
}
