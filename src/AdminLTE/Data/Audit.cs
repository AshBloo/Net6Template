using System;

namespace AdminLTE.Data
{
    public class Audit
    {
        public int Id { get; set; }                    /*Log id*/
        public Guid Guid { get; set; }                    /*Log guid*/
        public DateTime AuditDateTimeUtc { get; set; }  /*Log time*/
        public string AuditType { get; set; }           /*Create, Update or Delete*/
        public string AuditUser { get; set; }           /*Log User*/
        public string TableName { get; set; }           /*Table where rows been created/updated/deleted*/
        public string KeyValues { get; set; }           /*Pk and it's values*/
        public string OldValues { get; set; }           /*Changed column name and old value*/
        public string NewValues { get; set; }           /*Changed column name and current value*/
        public string ChangedColumns { get; set; }      /*Changed column names*/
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
    }

    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
}
