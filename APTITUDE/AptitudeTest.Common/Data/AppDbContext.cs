using AptitudeTest.Core.Entities.Admin;
using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Entities.Questions;
using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Entities.Users;
using AptitudeTest.Core.ViewModels;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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
                    .IsUnicode(false);
                entity.Property(e => e.FatherName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(500);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.Status).HasDefaultValue(true);
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
    }
}
