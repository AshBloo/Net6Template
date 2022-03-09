using AdminLTE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace AdminLTE
{
    public interface IMopDbContext : IAuditDbContext, IDisposable
    {
        DbSet<Role> Role { get; set; }

        DatabaseFacade Database { get; }
        int SaveChanges(string userName);
    }
}
