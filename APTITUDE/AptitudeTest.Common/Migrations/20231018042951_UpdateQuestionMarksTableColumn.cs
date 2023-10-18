using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AptitudeTest.Common.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestionMarksTableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "QuestionMarks",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Marks",
                table: "QuestionMarks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserViewModel",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FatherName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PermanentAddress = table.Column<string>(type: "text", nullable: true),
                    Group = table.Column<int>(type: "integer", nullable: true),
                    TechnologyInterestedIn = table.Column<int>(type: "integer", nullable: true),
                    AppliedThrough = table.Column<int>(type: "integer", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    TechnologyName = table.Column<string>(type: "text", nullable: true),
                    RelationshipWithExistingEmployee = table.Column<string>(type: "text", nullable: true),
                    ACPCMeritRank = table.Column<int>(type: "integer", nullable: true),
                    GUJCETScore = table.Column<int>(type: "integer", nullable: true),
                    JEEScore = table.Column<int>(type: "integer", nullable: true),
                    PreferedLocation = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMarks_Marks",
                table: "QuestionMarks",
                column: "Marks",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserViewModel");

            migrationBuilder.DropIndex(
                name: "IX_QuestionMarks_Marks",
                table: "QuestionMarks");

            migrationBuilder.DropColumn(
                name: "Marks",
                table: "QuestionMarks");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "QuestionMarks",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);
        }
    }
}
