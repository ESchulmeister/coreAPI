using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace coreAPI.Data
{
    public partial class UserDBContext : DbContext
    {
        public UserDBContext()
        {
        }

        public UserDBContext(DbContextOptions<UserDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessType> AccessTypes { get; set; }
        public virtual DbSet<AppDatabaseRole> AppDatabaseRoles { get; set; }
        public virtual DbSet<AppPermission> AppPermissions { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserPermission> AppUserPermissions { get; set; }
        public virtual DbSet<AppUserSetting> AppUserSettings { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ContentType> ContentTypes { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=UserDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccessType>(entity =>
            {
                entity.HasKey(e => e.ActId)
                    .HasName("PK_AccessTypes");

                entity.ToTable("AccessType");

                entity.Property(e => e.ActId)
                    .ValueGeneratedNever()
                    .HasColumnName("actID");

                entity.Property(e => e.ActName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("actName");
            });

            modelBuilder.Entity<AppDatabaseRole>(entity =>
            {
                entity.HasKey(e => e.AdbId)
                    .HasName("PK_AppDatabaseRoles");

                entity.ToTable("AppDatabaseRole");

                entity.Property(e => e.AdbId).HasColumnName("adbID");

                entity.Property(e => e.AdbAccessKey)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("adbAccessKey");

                entity.Property(e => e.AdbActive)
                    .IsRequired()
                    .HasColumnName("adbActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AdbCreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adbCreatedBy");

                entity.Property(e => e.AdbCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("adbCreatedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AdbModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adbModifiedBy");

                entity.Property(e => e.AdbModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("adbModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AdbName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("adbName");

                entity.Property(e => e.AdbRoleName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("adbRoleName");

                entity.Property(e => e.AppId).HasColumnName("appID");
            });

            modelBuilder.Entity<AppPermission>(entity =>
            {
                entity.HasKey(e => e.ApId)
                    .HasName("PK_Permissions");

                entity.ToTable("AppPermission");

                entity.Property(e => e.ApId).HasColumnName("apID");

                entity.Property(e => e.ApActive)
                    .IsRequired()
                    .HasColumnName("apActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ApCreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apCreatedBy")
                    .HasDefaultValueSql("('admin')");

                entity.Property(e => e.ApCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("apCreatedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ApModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apModifiedBy")
                    .HasDefaultValueSql("('admin')");

                entity.Property(e => e.ApModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("apModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AppId)
                    .HasColumnName("appID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PermName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("permName");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.AppPermissions)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permission_Application");
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.ApuId)
                    .HasName("PK_ApplicationUser");

                entity.ToTable("AppUser");

                entity.HasIndex(e => new { e.UsrId, e.AppId }, "IX_AppUsers_Unique_usrID_appID")
                    .IsUnique();

                entity.Property(e => e.ApuId).HasColumnName("apuID");

                entity.Property(e => e.AppId).HasColumnName("appID");

                entity.Property(e => e.ApuAccessKey)
                    .IsRequired()
                    .HasColumnName("apuAccessKey");

                entity.Property(e => e.ApuActive)
                    .IsRequired()
                    .HasColumnName("apuActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ApuCreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apuCreatedBy");

                entity.Property(e => e.ApuCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("apuCreatedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ApuModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apuModifiedBy");

                entity.Property(e => e.ApuModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("apuModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsrId).HasColumnName("usrID");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUser_Applications");

                entity.HasOne(d => d.Usr)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.UsrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUser_User");
            });

            modelBuilder.Entity<AppUserPermission>(entity =>
            {
                entity.HasKey(e => e.PerId)
                    .HasName("PK_AppUserPermissions");

                entity.ToTable("AppUserPermission");

                entity.HasIndex(e => new { e.ApuId, e.ApId }, "IX_AppUserPermissions_Unique_apuID_apID")
                    .IsUnique();
                entity.Property(e => e.PerId).HasColumnName("perID");

                entity.Property(e => e.ActId).HasColumnName("actID");

                entity.Property(e => e.ApId).HasColumnName("apID");

                entity.Property(e => e.ApuId).HasColumnName("apuID");

                entity.Property(e => e.PerActive)
                    .IsRequired()
                    .HasColumnName("perActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PerCreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("perCreatedBy");

                entity.Property(e => e.PerCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("perCreatedDate");

                entity.Property(e => e.PerMetadata)
                    .IsUnicode(false)
                    .HasColumnName("perMetadata");

                entity.Property(e => e.PerModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("perModifiedBy");

                entity.Property(e => e.PerModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("perModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Act)
                    .WithMany(p => p.AppUserPermissions)
                    .HasForeignKey(d => d.ActId)
                    .HasConstraintName("FK_AppUserPermissions_AccessTypes");

                entity.HasOne(d => d.Ap)
                    .WithMany(p => p.AppUserPermissions)
                    .HasForeignKey(d => d.ApId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUserPermission_AppPermission");

                entity.HasOne(d => d.Apu)
                    .WithMany(p => p.AppUserPermissions)
                    .HasForeignKey(d => d.ApuId)
                    .HasConstraintName("FK_AppUserPermissions_AppUsers");
            });

            modelBuilder.Entity<AppUserSetting>(entity =>
            {
                entity.HasKey(e => e.SetId)
                    .HasName("PK_AppUserSettings");

                entity.ToTable("AppUserSetting");

                entity.Property(e => e.SetId).HasColumnName("setID");

                entity.Property(e => e.ApuId).HasColumnName("apuID");

                entity.Property(e => e.SetActive)
                    .IsRequired()
                    .HasColumnName("setActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SetContentTypeId).HasColumnName("setContentTypeID");

                entity.Property(e => e.SetCreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("setCreatedBy");

                entity.Property(e => e.SetCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("setCreatedDate");

                entity.Property(e => e.SetModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("setModifiedBy");

                entity.Property(e => e.SetModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("setModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SetName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("setName");

                entity.Property(e => e.SetValue)
                    .IsUnicode(false)
                    .HasColumnName("setValue");

                entity.HasOne(d => d.Apu)
                    .WithMany(p => p.AppUserSettings)
                    .HasForeignKey(d => d.ApuId)
                    .HasConstraintName("FK_AppUserSettings_AppUsers");

                entity.HasOne(d => d.SetContentType)
                    .WithMany(p => p.AppUserSettings)
                    .HasForeignKey(d => d.SetContentTypeId)
                    .HasConstraintName("FK_AppUserSetting_ContentType");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.AppId)
                    .HasName("PK_Applications");

                entity.ToTable("Application");

                entity.Property(e => e.AppId).HasColumnName("appID");

                entity.Property(e => e.AppActive)
                    .IsRequired()
                    .HasColumnName("appActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AppCreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("appCreatedBy");

                entity.Property(e => e.AppCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("appCreatedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AppFlags).HasColumnName("appFlags");

                entity.Property(e => e.AppModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("appModifiedBy");

                entity.Property(e => e.AppModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("appModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AppName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("appName");
            });

            modelBuilder.Entity<ContentType>(entity =>
            {
                entity.HasKey(e => e.CntId);

                entity.ToTable("ContentType");

                entity.Property(e => e.CntId)
                    .ValueGeneratedNever()
                    .HasColumnName("cntID");

                entity.Property(e => e.CntName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cntName");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UsrId)
                    .HasName("PK_Users");

                entity.ToTable("User");

                entity.HasIndex(e => e.UsrLogin, "UX_UsrLogin")
                    .IsUnique();

                entity.Property(e => e.UsrId).HasColumnName("usrID");

                entity.Property(e => e.UsrActive)
                    .IsRequired()
                    .HasColumnName("usrActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UsrClock)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usrClock");

                entity.Property(e => e.UsrCreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usrCreatedBy");

                entity.Property(e => e.UsrCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("usrCreatedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsrEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("usrEmail");

                entity.Property(e => e.UsrFirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("usrFirstName");

                entity.Property(e => e.UsrLastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("usrLastName");

                entity.Property(e => e.UsrLogin)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usrLogin");

                entity.Property(e => e.UsrModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usrModifiedBy");

                entity.Property(e => e.UsrModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("usrModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsrStateId).HasColumnName("usrStateID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
