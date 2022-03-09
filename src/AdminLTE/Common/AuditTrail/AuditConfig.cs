using AdminLTE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLTE.Common.AuditTrail
{
    internal class AuditConfig : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Audit");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.AuditDateTimeUtc)
                .HasColumnName("audit_datetime_utc");
            entity.Property(e => e.AuditType)
                .HasColumnName("audit_type");
            entity.Property(e => e.AuditUser)
                .HasColumnName("audit_user");        
            entity.Property(e => e.TableName)
                .HasColumnName("table_name");
            entity.Property(e => e.KeyValues)
                .HasColumnName("key_values");
            entity.Property(e => e.OldValues)
                .HasColumnName("old_values");
            entity.Property(e => e.NewValues)
                .HasColumnName("new_values");
            entity.Property(e => e.ChangedColumns)
                .HasColumnName("changed_columns");
        }
    }
}