﻿using AptitudeTest.Core.Entities.Admin;
using AptitudeTest.Core.Entities.Candidate;
using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Entities.Questions;
using AptitudeTest.Core.Entities.Setting;
using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Entities.Users;
using AptitudeTest.Core.ViewModels;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace APTITUDETEST.Common.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__USER_ID");
                entity.ToTable("User");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.FirstName)
              .HasMaxLength(50)
              .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false).IsRequired(false);
                entity.Property(e => e.FatherName)
                    .HasMaxLength(50)
                    .IsUnicode(false).IsRequired(false);
                entity.Property(e => e.Email).HasMaxLength(500);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.IsProfileEdited).HasDefaultValue(false);
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(255);
                entity.Property(e => e.PermanentAddress1)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.PermanentAddress2)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.RelationshipWithExistingEmployee)
                   .HasMaxLength(300)
                   .IsUnicode(false);
                entity.Property(e => e.StateId).IsRequired(false);
                entity.Property(e => e.TechnologyInterestedIn).IsRequired(false);
                entity.Property(e => e.CityName).IsRequired(false).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.Gender).IsRequired(false);
                entity.Property(e => e.SessionId).IsRequired(false);
                entity.Property(e => e.SessionId).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.IsImported).HasDefaultValue(false);
                entity.Property(e => e.IsTestGenerated).HasDefaultValue(false);

            });

            modelBuilder.Entity<UserFamily>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__USERFAMILY_ID");
                entity.ToTable("UserFamily");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Occupation)
                    .HasMaxLength(300)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.Qualification)
                   .HasMaxLength(300)
                   .IsUnicode(false);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });

            modelBuilder.Entity<UserAcademics>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__USERACADEMICS_ID");
                entity.ToTable("UserAcademics");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.University)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.DurationFromMonth)
                   .HasMaxLength(10)
                   .IsUnicode(false);
                entity.Property(e => e.DurationFromYear)
                   .HasMaxLength(10)
                   .IsUnicode(false);
                entity.Property(e => e.DurationToYear)
                   .HasMaxLength(10)
                   .IsUnicode(false);
                entity.Property(e => e.DurationToMonth)
                   .HasMaxLength(10)
                   .IsUnicode(false);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<UserViewModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<MasterCollege>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MASTERCOLLEGE_ID");
                entity.ToTable("MasterCollege");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });

            modelBuilder.Entity<MasterGroup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MASTERGROUP_ID");
                entity.ToTable("MasterGroup");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.IsDefault).HasDefaultValue(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });

            modelBuilder.Entity<MasterDegree>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MASTERDEGREE_ID");
                entity.ToTable("MasterDegree");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Level).HasDefaultValue(1);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.IsEditable).HasDefaultValue(true);
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });
            modelBuilder.Entity<MasterStream>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MASTERSTREAM_ID");
                entity.ToTable("MasterStream");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });
            modelBuilder.Entity<MasterTechnology>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MASTERTECHNOLOGY_ID");
                entity.ToTable("MasterTechnology");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });

            modelBuilder.Entity<QuestionModule>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__QUESTIONMODULE_ID");
                entity.ToTable("QuestionModule");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });
            modelBuilder.Entity<QuestionMarks>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__QUESTIONMARKS_ID");
                entity.ToTable("QuestionMarks");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.HasIndex(e => e.Marks).IsUnique();
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__QUESTION_ID");
                entity.ToTable("Question");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.QuestionText)
                .HasMaxLength(2000)
                .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.ParentId).IsRequired(false);
                entity.Property(e => e.Sequence).IsRequired(false);
            });
            modelBuilder.Entity<QuestionOptions>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__QUESTIONOPTIONS_ID");
                entity.ToTable("QuestionOptions");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.OptionData)
                .HasColumnType("text")
                .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });
            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__STATE_ID");
                entity.ToTable("State");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CITY_ID");
                entity.ToTable("City");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ADMIN_ID");
                entity.ToTable("Admin");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.IsSuperAdmin).IsRequired(false).HasDefaultValue(false);
            });
            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TEST_ID");
                entity.ToTable("Test");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
                entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.Status).HasDefaultValue(1);
                entity.Property(e => e.IsRandomQuestion).HasDefaultValue(true);
                entity.Property(e => e.IsRandomAnswer).HasDefaultValue(true);
                entity.Property(e => e.IsLogoutWhenTimeExpire).HasDefaultValue(true);
                entity.Property(e => e.IsQuestionsMenu).HasDefaultValue(true);
                entity.Property(e => e.NegativeMarkingPercentage).HasDefaultValue(0);

            });
            modelBuilder.Entity<TestQuestions>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TESTQUESTION_ID");
                entity.ToTable("TestQuestions");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.TestId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.TopicId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.NoOfQuestions)
                 .HasMaxLength(10)
                 .IsUnicode(false);
                entity.Property(e => e.Weightage)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });
            modelBuilder.Entity<TestQuestionsCount>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TESTQUESTIONCOUNT_ID");
                entity.ToTable("TestQuestionsCount");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.TestQuestionId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.QuestionType)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.OneMarks)
                 .HasMaxLength(10)
                 .IsUnicode(false);
                entity.Property(e => e.TwoMarks)
                 .HasMaxLength(10)
                 .IsUnicode(false);
                entity.Property(e => e.ThreeMarks)
                 .HasMaxLength(10)
                 .IsUnicode(false);
                entity.Property(e => e.FourMarks)
                 .HasMaxLength(10)
                 .IsUnicode(false);
                entity.Property(e => e.FiveMarks)
                 .HasMaxLength(10)
                 .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });

            modelBuilder.Entity<TempUserTest>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TEMPUSERTEST_ID");
                entity.ToTable("TempUserTest");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.TestId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.Status)
                 .HasDefaultValue(true)
                 .IsUnicode(false);
                entity.Property(e => e.IsFinished)
                 .HasDefaultValue(false)
                 .IsUnicode(false);
                entity.Property(e => e.TimeRemaining)
                 .HasMaxLength(10)
                 .IsUnicode(false);
                entity.Property(e => e.IsAdminApproved)
                 .HasDefaultValue(true)
                 .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsStarted).HasDefaultValue(false);
            });

            modelBuilder.Entity<UserTest>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_USERTEST_ID");
                entity.ToTable("UserTest");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.TestId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.Status)
                 .HasDefaultValue(true)
                 .IsUnicode(false);
                entity.Property(e => e.IsFinished)
                 .HasDefaultValue(false)
                 .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<TempUserTestResult>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TEMPUSERTESTRESULT_ID");
                entity.ToTable("TempUserTestResult");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.UserTestId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.QuestionId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.UserAnswers)
                 .HasMaxLength(2000)
                 .IsUnicode(false);
                entity.Property(e => e.IsAttended)
                 .HasDefaultValue(false)
                 .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<UserTestResult>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_USERTESTRESULT_ID");
                entity.ToTable("UserTestResult");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.UserTestId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.QuestionId)
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.UserAnswers)
                 .HasMaxLength(2000)
                 .IsUnicode(false);
                entity.Property(e => e.IsAttended)
                 .HasDefaultValue(false)
                 .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<SettingConfigurations>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("settingconfigurations_pkey");
                entity.ToTable("SettingConfigurations");
                entity.Property(e => e.Id);
                entity.Property(e => e.UserRegistration);
                entity.Property(e => e.Camera);
                entity.Property(e => e.IntervalForScreenCapture).HasDefaultValue(10);
                entity.Property(e => e.CutOff).HasDefaultValue(0);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserFamily> UserFamily { get; set; }
        public DbSet<UserAcademics> UserAcademics { get; set; }
        public DbSet<MasterCollege> MasterCollege { get; set; }
        public DbSet<MasterDegree> MasterDegree { get; set; }
        public DbSet<MasterStream> MasterStream { get; set; }
        public DbSet<MasterTechnology> MasterTechnology { get; set; }
        public DbSet<MasterGroup> MasterGroup { get; set; }
        public DbSet<QuestionModule> QuestionModule { get; set; }
        public DbSet<QuestionMarks> QuestionMarks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOptions> QuestionOptions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestQuestions> TestQuestions { get; set; }
        public DbSet<TestQuestionsCount> TestQuestionsCount { get; set; }
        public DbSet<TempUserTest> TempUserTests { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<TempUserTestResult> TempUserTestResult { get; set; }
        public DbSet<UserTestResult> UserTestResult { get; set; }
        public DbSet<SettingConfigurations> SettingConfigurations { get; set; }
    }
}
