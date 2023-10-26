using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AptitudeTest.Common.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterCollege",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MASTERCOLLEGE_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterDegree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MASTERDEGREE_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MASTERGROUP_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterTechnology",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MASTERTECHNOLOGY_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Topic = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    QuestionText = table.Column<string>(type: "character varying(2000)", unicode: false, maxLength: 2000, nullable: false),
                    QuestionType = table.Column<int>(type: "integer", nullable: false),
                    OptionType = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QUESTION_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Marks = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QUESTIONMARKS_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QUESTIONMODULE_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__STATE_ID", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "MasterLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Location = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CollegeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MASTERLOCATION_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterLocation_MasterCollege_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "MasterCollege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterStream",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    DegreeId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MASTERSTREAM_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterStream_MasterDegree_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "MasterDegree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    FatherName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    PermanentAddress1 = table.Column<string>(type: "character varying(1000)", unicode: false, maxLength: 1000, nullable: true),
                    PermanentAddress2 = table.Column<string>(type: "character varying(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Pincode = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    AppliedThrough = table.Column<int>(type: "integer", nullable: true),
                    TechnologyInterestedIn = table.Column<int>(type: "integer", nullable: true),
                    ACPCMeritRank = table.Column<int>(type: "integer", nullable: true),
                    GUJCETScore = table.Column<int>(type: "integer", nullable: true),
                    JEEScore = table.Column<int>(type: "integer", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    PreferedLocation = table.Column<int>(type: "integer", nullable: true),
                    RelationshipWithExistingEmployee = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    RoleId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_MasterGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MasterGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_MasterTechnology_TechnologyInterestedIn",
                        column: x => x.TechnologyInterestedIn,
                        principalTable: "MasterTechnology",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    OptionData = table.Column<string>(type: "text", unicode: false, nullable: false),
                    IsAnswer = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QUESTIONOPTIONS_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CITY_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAcademics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DegreeId = table.Column<int>(type: "integer", nullable: false),
                    StreamId = table.Column<int>(type: "integer", nullable: false),
                    Maths = table.Column<float>(type: "real", nullable: true),
                    Physics = table.Column<float>(type: "real", nullable: true),
                    Grade = table.Column<float>(type: "real", nullable: false),
                    University = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    DurationFromYear = table.Column<int>(type: "integer", unicode: false, maxLength: 10, nullable: false),
                    DurationFromMonth = table.Column<int>(type: "integer", unicode: false, maxLength: 10, nullable: false),
                    DurationToYear = table.Column<int>(type: "integer", unicode: false, maxLength: 10, nullable: false),
                    DurationToMonth = table.Column<int>(type: "integer", unicode: false, maxLength: 10, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USERACADEMICS_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAcademics_MasterDegree_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "MasterDegree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAcademics_MasterStream_StreamId",
                        column: x => x.StreamId,
                        principalTable: "MasterStream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAcademics_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFamily",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FamilyPerson = table.Column<int>(type: "integer", nullable: false),
                    Qualification = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: false),
                    Occupation = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USERFAMILY_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFamily_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_StateId",
                table: "City",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterLocation_CollegeId",
                table: "MasterLocation",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterStream_DegreeId",
                table: "MasterStream",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMarks_Marks",
                table: "QuestionMarks",
                column: "Marks",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_GroupId",
                table: "User",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TechnologyInterestedIn",
                table: "User",
                column: "TechnologyInterestedIn");

            migrationBuilder.CreateIndex(
                name: "IX_UserAcademics_DegreeId",
                table: "UserAcademics",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAcademics_StreamId",
                table: "UserAcademics",
                column: "StreamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAcademics_UserId",
                table: "UserAcademics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFamily_UserId",
                table: "UserFamily",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "MasterLocation");

            migrationBuilder.DropTable(
                name: "QuestionMarks");

            migrationBuilder.DropTable(
                name: "QuestionModule");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "UserAcademics");

            migrationBuilder.DropTable(
                name: "UserFamily");

            migrationBuilder.DropTable(
                name: "UserViewModel");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "MasterCollege");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "MasterStream");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "MasterDegree");

            migrationBuilder.DropTable(
                name: "MasterGroup");

            migrationBuilder.DropTable(
                name: "MasterTechnology");
        }
    }
}
