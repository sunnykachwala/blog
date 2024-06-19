namespace Np.DAL.Context
{
    using Microsoft.EntityFrameworkCore;
    using Np.Common;
    using Np.DAL.Domain;

    public class NewsPortalContext : DbContext
    {
        public NewsPortalContext() : base() { }
        public NewsPortalContext(DbContextOptions<NewsPortalContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        public int AuditSaveChanges(string userId, int ActivityLogId)
        {
            OnBeforeSaveChanges(userId, ActivityLogId);
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region App Settings
            modelBuilder.Entity<AppSetting>()
          .Property(e => e.Id)
          .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<AppSetting>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<AppSetting>()
            .Property(e => e.AppName)
            .HasDefaultValueSql("app_name()");

            #endregion

            #region Activity Log
            modelBuilder.Entity<ActivityLog>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<ActivityLog>()
            .Property(e => e.AppName)
            .HasDefaultValueSql("app_name()");

            #endregion

            #region Article Category

            modelBuilder.Entity<ArticleCategory>()
            .HasKey(ac => new { ac.ArticleId, ac.CategoryId });

            modelBuilder.Entity<ArticleCategory>()
            .HasOne(ac => ac.Article)
            .WithMany(a => a.ArticleCategories)
            .HasForeignKey(ac => ac.ArticleId);

            modelBuilder.Entity<ArticleCategory>()
            .HasOne(ac => ac.Category)
            .WithMany(c => c.ArticleCategories)
            .HasForeignKey(ac => ac.CategoryId);
            #endregion

            #region ArticleTag
            modelBuilder.Entity<ArticleTag>()
                .HasKey(at => new { at.ArticleId, at.TagId });

            modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTags)
                .HasForeignKey(at => at.ArticleId);

            modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId);
            #endregion

            #region Article

            modelBuilder.Entity<Article>()
            .Property(e => e.ArticleId)
            .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Article>()
            .HasIndex(e => e.Slug)
            .IsUnique();

            modelBuilder.Entity<Article>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<Article>()
            .Property(e => e.AppName)
            .HasDefaultValueSql("app_name()");

            #endregion

            #region Audit Record
            modelBuilder.Entity<AuditRecord>()
            .Property(e => e.AuditRecordGuid)
            .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<AuditRecord>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<AuditRecord>()
           .Property(e => e.AppName)
           .HasDefaultValueSql("app_name()");
            #endregion

            #region Article View
            modelBuilder.Entity<ArticleView>()
           .Property(e => e.SystemUser)
           .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<ArticleView>()
            .Property(e => e.AppName)
            .HasDefaultValueSql("app_name()");
            #endregion

            #region Category

            modelBuilder.Entity<Category>()
            .HasIndex(e => e.Slug)
            .IsUnique();

            modelBuilder.Entity<Category>()
            .HasIndex(e => e.Title)
            .IsUnique();

            modelBuilder.Entity<Category>()
           .Property(e => e.Id)
           .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Category>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<Category>()
           .Property(e => e.AppName)
           .HasDefaultValueSql("app_name()");
            //modelBuilder.Entity<Category>()
            //.HasOne(c => c.ParentCategory)
            //.WithMany(c => c.Subcategories)
            //.HasForeignKey(c => c.ParentCategoryId)
            //.OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade delete
            #endregion

            #region Organisation
            modelBuilder.Entity<Organisation>()
            .Property(e => e.OrganisationGuid)
            .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Organisation>()
            .HasIndex(e => e.OrganisationName)
            .IsUnique();

            modelBuilder.Entity<Organisation>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<Organisation>()
           .Property(e => e.AppName)
           .HasDefaultValueSql("app_name()");
            #endregion

            #region User Role Permission Mapping
            modelBuilder.Entity<RolePermissionMapping>()
           .Property(e => e.RolePermissionMappingId)
           .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<RolePermissionMapping>()
           .HasIndex(e => new { e.UserPermissionId, e.UserRoleId })
           .IsUnique();
            #endregion

            #region Tag
            modelBuilder.Entity<Tag>()
           .HasIndex(e => e.Slug)
           .IsUnique();

            modelBuilder.Entity<Tag>()
            .HasIndex(e => e.TagName)
            .IsUnique();

            modelBuilder.Entity<Tag>()
           .Property(e => e.Id)
           .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Tag>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<Tag>()
           .Property(e => e.AppName)
           .HasDefaultValueSql("app_name()");

            #endregion

            #region UrlRecord
            modelBuilder.Entity<UrlRecord>()
           .Property(e => e.Id)
           .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<UrlRecord>()
            .HasIndex(e => e.Slug)
            .IsUnique();

            #endregion

            #region User
            modelBuilder.Entity<AdminUser>()
            .Property(e => e.UserGuid)
            .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<AdminUser>()
            .HasIndex(e => new { e.UserEmail, e.OrganisationGuid })
            .IsUnique();

            modelBuilder.Entity<AdminUser>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<AdminUser>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<AdminUser>()
           .Property(e => e.AppName)
           .HasDefaultValueSql("app_name()");

            #endregion

            #region User Role
            modelBuilder.Entity<AdminUserRole>()
            .Property(e => e.UserRoleGuid)
            .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<AdminUserRole>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<AdminUserRole>()
           .Property(e => e.AppName)
           .HasDefaultValueSql("app_name()");
            #endregion

            #region User Permission
            modelBuilder.Entity<UserPermission>()
           .Property(e => e.UserPermissionId)
           .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<UserPermission>()
            .Property(e => e.SystemUser)
            .HasDefaultValueSql("suser_sname()");

            modelBuilder.Entity<UserPermission>()
           .Property(e => e.AppName)
           .HasDefaultValueSql("app_name()");
            #endregion

        }

        private void OnBeforeSaveChanges(string userId, int ActivityLogId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditRecord || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserGuid = userId,
                    ActivityLogId = ActivityLogId
                };
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        auditEntry.KeyValue = Convert.ToString(property.CurrentValue);
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                if (property.OriginalValue != null && property.CurrentValue != null)
                                {
                                    if (!property.OriginalValue.Equals(property.CurrentValue))
                                    {
                                        auditEntry.ChangedColumns.Add(propertyName);
                                        auditEntry.AuditType = AuditType.Update;
                                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                                    }
                                }
                                else
                                {
                                    auditEntry.ChangedColumns.Add(propertyName);
                                    auditEntry.AuditType = AuditType.Update;
                                    auditEntry.OldValues[propertyName] = property.OriginalValue;
                                    auditEntry.NewValues[propertyName] = property.CurrentValue;
                                }
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditRecords.Add(auditEntry.ToAuditRecord());
            }
        }
        public DbSet<AppSetting> AppSetting { get; set; }
        public DbSet<AuditRecord> AuditRecords { get; set; }
        public DbSet<AdminUser> AdminUser { get; set; }
        public DbSet<AdminUserRole> AdminUserRole { get; set; }
        public DbSet<ActivityLog> ActivityLog { get; set; }
        public DbSet<ArticleCategory> ArticleCategory { get; set; }
        public DbSet<ArticleTag> ArticleTag { get; set; }
        public DbSet<ArticleComment> ArticleComment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<LoginResetHistory> LoginResetHistory { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<RolePermissionMapping> RolePermissionMapping { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<UrlRecord> UrlRecord { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
    }
}
