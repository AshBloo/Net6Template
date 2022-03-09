using AdminLTE.Common.AuditTrail;
using AdminLTE.Models;
using Coravel.Pro.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdminLTE.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IMopDbContext, ICoravelProDbContext
    {
        public DbSet<UserAudit> UserAuditEvents { get; set; }
        public virtual DbSet<Audit> Audit { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        //public virtual DbSet<HangfireJobs> HangfireJobs { get; set; }
        public DbSet<CoravelJobHistory> Coravel_JobHistory { get; set; }
        public DbSet<CoravelScheduledJob> Coravel_ScheduledJobs { get; set; }
        public DbSet<CoravelScheduledJobHistory> Coravel_ScheduledJobHistory { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());

            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

        }


        public virtual int SaveChanges(string userName)
        {
            if (userName == null)
            {
                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }

            new AuditHelper(this).AddAuditLogs(userName);
            var result = SaveChanges();
            return result;
        }


        public Task<int> SaveChangesAsync(string userName)
        {
            if (userName == null)
            {
                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }

            new AuditHelper(this).AddAuditLogs(userName);
            var result = SaveChangesAsync();
            return result;
        }
    }
}
