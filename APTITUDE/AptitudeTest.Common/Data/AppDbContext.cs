using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Entities.Users;
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
                    .IsUnicode(false);
                entity.Property(e => e.FatherName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(500);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.Level).HasDefaultValue(1);
                entity.Property(e => e.Password)
                    .HasMaxLength(255);
                entity.Property(e => e.PermanentAddress)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.RelationshipWithExistingEmployee)
                   .HasMaxLength(300)
                   .IsUnicode(false);


            });

            modelBuilder.Entity<User>().HasMany(e => e.UserAcademics).WithOne(e => e.Users).HasForeignKey(e => e.UserId).IsRequired();
            modelBuilder.Entity<User>().HasMany(e => e.UserFamily).WithOne(e => e.Users).HasForeignKey(e => e.UserId).IsRequired();


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
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedBy).HasDefaultValue(1);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });

            modelBuilder.Entity<MasterLocation>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MASTERLOCATION_ID");
                entity.ToTable("MasterLocation");
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                entity.Property(e => e.Location)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasDefaultValue(true);
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
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserFamily> UserFamily { get; set; }
        public DbSet<UserAcademics> UserAcademics { get; set; }
        public DbSet<MasterCollege> MasterCollege { get; set; }
        public DbSet<MasterDegree> MasterDegree { get; set; }
        public DbSet<MasterLocation> MasterLocation { get; set; }
        public DbSet<MasterStream> MasterStream { get; set; }
        public DbSet<MasterTechnology> MasterTechnology { get; set; }
        public DbSet<MasterGroup> MasterGroup { get; set; }
    }
}
