using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment2.Migrations
{
    public partial class coursenav : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcomes_Courses_CourseId",
                table: "LearningOutcomes");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "LearningOutcomes",
                newName: "CourseNavigationCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_LearningOutcomes_CourseId",
                table: "LearningOutcomes",
                newName: "IX_LearningOutcomes_CourseNavigationCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningOutcomes_Courses_CourseNavigationCourseId",
                table: "LearningOutcomes",
                column: "CourseNavigationCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcomes_Courses_CourseNavigationCourseId",
                table: "LearningOutcomes");

            migrationBuilder.RenameColumn(
                name: "CourseNavigationCourseId",
                table: "LearningOutcomes",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_LearningOutcomes_CourseNavigationCourseId",
                table: "LearningOutcomes",
                newName: "IX_LearningOutcomes_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningOutcomes_Courses_CourseId",
                table: "LearningOutcomes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
