﻿// <auto-generated />
using System;
using APTITUDETEST.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AptitudeTest.Common.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231018042951_UpdateQuestionMarksTableColumn")]
    partial class UpdateQuestionMarksTableColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("APTITUDETEST.Core.Entities.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("ACPCMeritRank")
                        .HasColumnType("integer");

                    b.Property<int?>("AppliedThrough")
                        .HasColumnType("integer");

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("FatherName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("GUJCETScore")
                        .HasColumnType("integer");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer");

                    b.Property<int?>("Group")
                        .HasColumnType("integer");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int?>("JEEScore")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PermanentAddress")
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("character varying(1000)");

                    b.Property<long?>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.Property<int?>("PreferedLocation")
                        .HasColumnType("integer");

                    b.Property<string>("RelationshipWithExistingEmployee")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("character varying(300)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("integer");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("TechnologyInterestedIn")
                        .HasColumnType("integer");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__USER_ID");

                    b.HasIndex("Group");

                    b.HasIndex("TechnologyInterestedIn");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterCollege", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__MASTERCOLLEGE_ID");

                    b.ToTable("MasterCollege", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterDegree", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool?>("IsEditable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__MASTERDEGREE_ID");

                    b.ToTable("MasterDegree", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__MASTERGROUP_ID");

                    b.ToTable("MasterGroup", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("CollegeId")
                        .HasColumnType("integer");

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__MASTERLOCATION_ID");

                    b.HasIndex("CollegeId");

                    b.ToTable("MasterLocation", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterStream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("DegreeId")
                        .HasColumnType("integer");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__MASTERSTREAM_ID");

                    b.HasIndex("DegreeId");

                    b.ToTable("MasterStream", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterTechnology", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__MASTERTECHNOLOGY_ID");

                    b.ToTable("MasterTechnology", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Questions.QuestionMarks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("Marks")
                        .HasColumnType("integer");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__QUESTIONMARKS_ID");

                    b.HasIndex("Marks")
                        .IsUnique();

                    b.ToTable("QuestionMarks", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Questions.QuestionModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("character varying(255)");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id")
                        .HasName("PK__QUESTIONMODULE_ID");

                    b.ToTable("QuestionModule", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Users.UserAcademics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("DegreeId")
                        .HasColumnType("integer");

                    b.Property<int>("DurationFromMonth")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<int>("DurationFromYear")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<int>("DurationToMonth")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<int>("DurationToYear")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<float>("Grade")
                        .HasColumnType("real");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<float?>("Maths")
                        .HasColumnType("real");

                    b.Property<float?>("Physics")
                        .HasColumnType("real");

                    b.Property<int>("StreamId")
                        .HasColumnType("integer");

                    b.Property<string>("University")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("PK__USERACADEMICS_ID");

                    b.HasIndex("DegreeId");

                    b.HasIndex("StreamId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAcademics", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Users.UserFamily", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("FamilyPerson")
                        .HasColumnType("integer");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Occupation")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("Qualification")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("character varying(300)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("PK__USERFAMILY_ID");

                    b.HasIndex("UserId");

                    b.ToTable("UserFamily", (string)null);
                });

            modelBuilder.Entity("AptitudeTest.Core.ViewModels.UserViewModel", b =>
                {
                    b.Property<int?>("ACPCMeritRank")
                        .HasColumnType("integer");

                    b.Property<int?>("AppliedThrough")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FatherName")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int?>("GUJCETScore")
                        .HasColumnType("integer");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer");

                    b.Property<int?>("Group")
                        .HasColumnType("integer");

                    b.Property<int?>("JEEScore")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<int?>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("PermanentAddress")
                        .HasColumnType("text");

                    b.Property<long?>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.Property<int?>("PreferedLocation")
                        .HasColumnType("integer");

                    b.Property<string>("RelationshipWithExistingEmployee")
                        .HasColumnType("text");

                    b.Property<bool?>("Status")
                        .HasColumnType("boolean");

                    b.Property<int?>("TechnologyInterestedIn")
                        .HasColumnType("integer");

                    b.Property<string>("TechnologyName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.ToTable("UserViewModel");
                });

            modelBuilder.Entity("APTITUDETEST.Core.Entities.Users.User", b =>
                {
                    b.HasOne("AptitudeTest.Core.Entities.Master.MasterGroup", "MasterGroups")
                        .WithMany()
                        .HasForeignKey("Group");

                    b.HasOne("AptitudeTest.Core.Entities.Master.MasterTechnology", "MasterTechnologies")
                        .WithMany()
                        .HasForeignKey("TechnologyInterestedIn");

                    b.Navigation("MasterGroups");

                    b.Navigation("MasterTechnologies");
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterLocation", b =>
                {
                    b.HasOne("AptitudeTest.Core.Entities.Master.MasterCollege", "MasterColleges")
                        .WithMany()
                        .HasForeignKey("CollegeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MasterColleges");
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Master.MasterStream", b =>
                {
                    b.HasOne("AptitudeTest.Core.Entities.Master.MasterDegree", "MasterDegrees")
                        .WithMany()
                        .HasForeignKey("DegreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MasterDegrees");
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Users.UserAcademics", b =>
                {
                    b.HasOne("AptitudeTest.Core.Entities.Master.MasterDegree", "MasterDegrees")
                        .WithMany()
                        .HasForeignKey("DegreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AptitudeTest.Core.Entities.Master.MasterStream", "MasterStreams")
                        .WithMany()
                        .HasForeignKey("StreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APTITUDETEST.Core.Entities.Users.User", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MasterDegrees");

                    b.Navigation("MasterStreams");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("AptitudeTest.Core.Entities.Users.UserFamily", b =>
                {
                    b.HasOne("APTITUDETEST.Core.Entities.Users.User", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
