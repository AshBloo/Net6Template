using AdminLTE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AdminLTE
{
    public interface IAuditDbContext
    {
        DbSet<Audit> Audit { get; set; }
        ChangeTracker ChangeTracker { get; }
    }
}