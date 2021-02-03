using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Infrastructure.Data
{
    public class CampaignSchedulerContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public CampaignSchedulerContext(DbContextOptions<CampaignSchedulerContext> options) : base(options)
        {

        }
        public virtual DbSet<SdrGrouping> SdrGroupings { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<CampaignAssignment> CampaignAssignments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<AppUserToken> AppUserTokens { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Scheduler> Schedulers { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Payroll> Payrolls { get; set; }
        public virtual DbSet<CampaignPricing> CampaignPricings { get; set; }
        public virtual DbSet<Wage> Wages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
