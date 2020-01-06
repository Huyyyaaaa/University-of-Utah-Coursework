using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment2.Migrations
{
    public partial class fixlos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcomes_Courses_CourseNavigationCourseId",
                table: "LearningOutcomes");

            migrationBuilder.RenameColumn(
                name: "CourseNavigationCourseId",
                table: "LearningOutcomes",
                newName: "number");

            migrationBuilder.RenameColumn(
                name: "CourseInstanceID",
                table: "LearningOutcomes",
                newName: "CourseNum");

            migrationBuilder.RenameIndex(
                name: "IX_LearningOutcomes_CourseNavigationCourseId",
                table: "LearningOutcomes",
                newName: "IX_LearningOutcomes_number");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningOutcomes_Courses_number",
                table: "LearningOutcomes",
                column: "number",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcomes_Courses_number",
                table: "LearningOutcomes");

            migrationBuilder.RenameColumn(
                name: "number",
                table: "LearningOutcomes",
                newName: "CourseNavigationCourseId");

            migrationBuilder.RenameColumn(
                name: "CourseNum",
                table: "LearningOutcomes",
                newName: "CourseInstanceID");

            migrationBuilder.RenameIndex(
                name: "IX_LearningOutcomes_number",
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
    }
}
