using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AptitudeTest.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndMasterTables : Migration
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MASTERTECHNOLOGY_ID", x => x.Id);
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
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
                    LastName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    FatherName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    Password = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    PermanentAddress = table.Column<string>(type: "character varying(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Group = table.Column<int>(type: "integer", nullable: true),
                    AppliedThrough = table.Column<int>(type: "integer", nullable: true),
                    TechnologyInterestedIn = table.Column<int>(type: "integer", nullable: true),
                    ACPCMeritRank = table.Column<int>(type: "integer", nullable: true),
                    GUJCETScore = table.Column<int>(type: "integer", nullable: true),
                    JEEScore = table.Column<int>(type: "integer", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    PreferedLocation = table.Column<int>(type: "integer", nullable: true),
                    RelationshipWithExistingEmployee = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    RoleId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USER_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_MasterGroup_Group",
                        column: x => x.Group,
                        principalTable: "MasterGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_MasterTechnology_TechnologyInterestedIn",
                        column: x => x.TechnologyInterestedIn,
                        principalTable: "MasterTechnology",
                        principalColumn: "Id");
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
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
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
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
                name: "IX_MasterLocation_CollegeId",
                table: "MasterLocation",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterStream_DegreeId",
                table: "MasterStream",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Group",
                table: "User",
                column: "Group");

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
                name: "MasterLocation");

            migrationBuilder.DropTable(
                name: "UserAcademics");

            migrationBuilder.DropTable(
                name: "UserFamily");

            migrationBuilder.DropTable(
                name: "MasterCollege");

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
