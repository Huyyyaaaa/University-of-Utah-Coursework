using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment2.Migrations
{
    public partial class coursenotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseNote",
                columns: table => new
                {
                    CourseNoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    note = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    modifiedDate = table.Column<DateTime>(nullable: false),
                    CourseNum = table.Column<int>(nullable: false),
                    number = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseNote", x => x.CourseNoteId);
                    table.ForeignKey(
                        name: "FK_CourseNote_Courses_number",
                        column: x => x.number,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcomeNote",
                columns: table => new
                {
                    LearningOutcomeNoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    note = table.Column<string>(nullable: true),
                    lastEditChair = table.Column<bool>(nullable: false),
                    modifiedDate = table.Column<DateTime>(nullable: false),
                    LOId = table.Column<int>(nullable: false),
                    LearningOutcomeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcomeNote", x => x.LearningOutcomeNoteId);
                    table.ForeignKey(
                        name: "FK_LearningOutcomeNote_LearningOutcomes_LearningOutcomeId",
                        column: x => x.LearningOutcomeId,
                        principalTable: "LearningOutcomes",
                        principalColumn: "LearningOutcomeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseNote_number",
                table: "CourseNote",
                column: "number");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomeNote_LearningOutcomeId",
                table: "LearningOutcomeNote",
                column: "LearningOutcomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseNote");

            migrationBuilder.DropTable(
                name: "LearningOutcomeNote");
        }
    }
}
