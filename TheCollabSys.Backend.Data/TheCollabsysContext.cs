using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data;

public partial class TheCollabsysContext : DbContext
{
    public TheCollabsysContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<DdActivity> DdActivities { get; set; }

    public virtual DbSet<DdAssignmentHistory> DdAssignmentHistories { get; set; }

    public virtual DbSet<DdAttribute> DdAttributes { get; set; }

    public virtual DbSet<DdAttributeValue> DdAttributeValues { get; set; }

    public virtual DbSet<DdClient> DD_Clients { get; set; }

    public virtual DbSet<DdClientRating> DdClientRatings { get; set; }

    public virtual DbSet<DdDomainMaster> DD_DomainMaster { get; set; }

    public virtual DbSet<DdEmployer> DD_Employers { get; set; }

    public virtual DbSet<DdEngineer> DD_Engineers { get; set; }

    public virtual DbSet<DdEngineerActivity> DdEngineerActivities { get; set; }

    public virtual DbSet<DdEngineerDailyAssignment> DdEngineerDailyAssignments { get; set; }

    public virtual DbSet<DdEngineerEquipment> DdEngineerEquipments { get; set; }

    public virtual DbSet<DdEngineerRating> DdEngineerRatings { get; set; }

    public virtual DbSet<DdEngineerSkill> DD_EngineerSkills { get; set; }

    public virtual DbSet<DdEngineerSoftware> DdEngineerSoftwares { get; set; }

    public virtual DbSet<DdEquipment> DdEquipments { get; set; }

    public virtual DbSet<DdLevelMaster> DdLevelMasters { get; set; }

    public virtual DbSet<DdProfile> DdProfiles { get; set; }

    public virtual DbSet<DdProfileConfigAction> DdProfileConfigActions { get; set; }

    public virtual DbSet<DdProfileConfigView> DdProfileConfigViews { get; set; }

    public virtual DbSet<DdProject> DD_Projects { get; set; }

    public virtual DbSet<DdProjectAssignment> DdProjectAssignments { get; set; }

    public virtual DbSet<DdProjectEquipment> DdProjectEquipments { get; set; }

    public virtual DbSet<DdProjectSkill> DD_ProjectSkills { get; set; }

    public virtual DbSet<DdProjectSoftware> DdProjectSoftwares { get; set; }

    public virtual DbSet<DdProjectStatus> DdProjectStatuses { get; set; }

    public virtual DbSet<DdProposalRole> DD_ProposalRoles { get; set; }

    public virtual DbSet<DdSkill> DD_Skills { get; set; }

    public virtual DbSet<DdSoftware> DdSoftwares { get; set; }

    public virtual DbSet<DdUser> DdUsers { get; set; }

    public virtual DbSet<DdUsersProfile> DdUsersProfiles { get; set; }

    public virtual DbSet<DdView> DdViews { get; set; }
    public virtual DbSet<Token> Token { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<AspNetUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<AspNetUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<DdActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId);
            entity.Property(e => e.ActivityName).HasMaxLength(100);
        });

        modelBuilder.Entity<DdAssignmentHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId);

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdAssignmentHistories)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.DdAssignmentHistories)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdAttribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId);
            entity.Property(e => e.AttributeId);
            entity.Property(e => e.AttributeName).HasMaxLength(255);
        });

        modelBuilder.Entity<DdAttributeValue>(entity =>
        {
            entity.HasKey(e => e.ValueId);

            entity.Property(e => e.EntityId)
                .HasMaxLength(450);

            entity.HasOne(d => d.Attribute).WithMany(p => p.DdAttributeValues)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdClient>(entity =>
        {
            entity.HasKey(e => e.ClientId);

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ClientName).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.DdClients)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<DdClientRating>(entity =>
        {
            entity.HasKey(e => e.RatingId);



            entity.HasOne(d => d.Client).WithMany(p => p.DdClientRatings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdClientRatings)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdDomainMaster>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Domain).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
        });

        modelBuilder.Entity<DdEmployer>(entity =>
        {
            entity.HasKey(e => e.EmployerId);

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.EmployerName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.DdEmployers)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<DdEngineer>(entity =>
        {
            entity.HasKey(e => e.EngineerId);

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EngineerName).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(450);

            entity.HasOne(d => d.Employer).WithMany(p => p.DdEngineers)
                .HasForeignKey(d => d.EmployerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.DdEngineers)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<DdEngineerActivity>(entity =>
        {
            entity.HasKey(e => e.EngineerActivityId);

            entity.HasOne(d => d.Activity).WithMany(p => p.DdEngineerActivities)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdEngineerActivities)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.DdEngineerActivities)
                .HasForeignKey(d => d.ProjectId);
        });

        modelBuilder.Entity<DdEngineerDailyAssignment>(entity =>
        {
            entity.HasKey(e => new { e.AssignmentDate, e.EngineerId });

            entity.HasOne(d => d.Activity).WithMany(p => p.DdEngineerDailyAssignments)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdEngineerDailyAssignments)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.DdEngineerDailyAssignments)
                .HasForeignKey(d => d.ProjectId);
        });

        modelBuilder.Entity<DdEngineerEquipment>(entity =>
        {
            entity.HasKey(e => new { e.EngineerId, e.EquipmentId });

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdEngineerEquipments)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Equipment).WithMany(p => p.DdEngineerEquipments)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Level).WithMany(p => p.DdEngineerEquipments)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdEngineerRating>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            entity.HasOne(d => d.Client).WithMany(p => p.DdEngineerRatings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdEngineerRatings)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdEngineerSkill>(entity =>
        {
            entity.HasKey(e => new { e.EngineerId, e.SkillId });

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdEngineerSkills)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Level).WithMany(p => p.DdEngineerSkills)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Skill).WithMany(p => p.DdEngineerSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdEngineerSoftware>(entity =>
        {
            entity.HasKey(e => new { e.EngineerId, e.SoftwareId });

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdEngineerSoftwares)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Level).WithMany(p => p.DdEngineerSoftwares)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Software).WithMany(p => p.DdEngineerSoftwares)
                .HasForeignKey(d => d.SoftwareId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdEquipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId);

            entity.Property(e => e.EquipmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<DdLevelMaster>(entity =>
        {
            entity.HasKey(e => e.LevelId);


            entity.Property(e => e.LevelDescription).HasMaxLength(255);
        });

        modelBuilder.Entity<DdProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId);

            entity.Property(e => e.ProfileName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DdProfileConfigAction>(entity =>
        {
            entity.HasKey(e => e.ActionId);

            entity.HasOne(d => d.FView).WithMany(p => p.DdProfileConfigActions)
                .HasForeignKey(d => d.FViewId);

            entity.HasOne(d => d.Profile).WithMany(p => p.DdProfileConfigActions)
                .HasForeignKey(d => d.ProfileId);
        });

        modelBuilder.Entity<DdProfileConfigView>(entity =>
        {
            entity.HasKey(e => e.ConfigId);

            entity.HasOne(d => d.Profile).WithMany(p => p.DdProfileConfigViews)
                .HasForeignKey(d => d.ProfileId);

            entity.HasOne(d => d.View).WithMany(p => p.DdProfileConfigViews)
                .HasForeignKey(d => d.ViewId);
        });

        modelBuilder.Entity<DdProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId);

            entity.Property(e => e.ProjectName).HasMaxLength(255);


            entity.HasOne(d => d.Client).WithMany(p => p.DdProjects)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Status).WithMany(p => p.DdProjects)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdProjectAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId);

            entity.HasOne(d => d.Engineer).WithMany(p => p.DdProjectAssignments)
                .HasForeignKey(d => d.EngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.DdProjectAssignments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdProjectEquipment>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.EquipmentId });

            entity.HasOne(d => d.Equipment).WithMany(p => p.DdProjectEquipments)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Level).WithMany(p => p.DdProjectEquipments)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.DdProjectEquipments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdProjectSkill>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.SkillId });

            entity.HasOne(d => d.Level).WithMany(p => p.DdProjectSkills)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.DdProjectSkills)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Skill).WithMany(p => p.DdProjectSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdProjectSoftware>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.SoftwareId });

            entity.HasOne(d => d.Level).WithMany(p => p.DdProjectSoftwares)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.DdProjectSoftwares)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Software).WithMany(p => p.DdProjectSoftwares)
                .HasForeignKey(d => d.SoftwareId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DdProjectStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<DdProposalRole>(entity =>
        {
            entity.HasKey(e => e.ProposalId);

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ProposalName).HasMaxLength(254);
            entity.Property(e => e.ProposalRoles).HasMaxLength(254);
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.Role).WithMany(p => p.DdProposalRoles)
                .HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<DdSkill>(entity =>
        {
            entity.HasKey(e => e.SkillId);

            entity.Property(e => e.SkillName).HasMaxLength(100);
        });

        modelBuilder.Entity<DdSoftware>(entity =>
        {
            entity.HasKey(e => e.SoftwareId);

            entity.Property(e => e.SoftwareName).HasMaxLength(100);
        });

        modelBuilder.Entity<DdUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DdUsersProfile>(entity =>
        {
            entity.HasKey(e => e.UserProfileId);

            entity.HasOne(d => d.Profile).WithMany(p => p.DdUsersProfiles)
                .HasForeignKey(d => d.ProfileId);

            entity.HasOne(d => d.User).WithMany(p => p.DdUsersProfiles)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<DdView>(entity =>
        {
            entity.HasKey(e => e.ViewId);

            entity.Property(e => e.ViewName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.RefreshToken)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        base.OnModelCreating(modelBuilder);
    }
}
