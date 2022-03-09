using AdminLTE.Common.AuditTrail;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AdminLTE.Data
{
    public abstract class MopDbContext : DbContext, IMopDbContext
    {
        public virtual DbSet<Audit> Audit { get; set; }
        public virtual DbSet<Role> Role { get; set; }


        public MopDbContext(DbContextOptions<MopDbContext> options)
            : base(options)
        {
        }

        protected MopDbContext() : base()
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
        }

        public virtual int SaveChanges(string userName)
        {
            new AuditHelper(this).AddAuditLogs(userName);
            var result = SaveChanges();
            return result;
        }

        public Task<int> SaveChangesAsync(string userName)
        {
            new AuditHelper(this).AddAuditLogs(userName);
            var result = SaveChangesAsync();
            return result;
        }
    }
}
