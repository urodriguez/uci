namespace Infrastructure.Crosscutting.Auditing
{
    public class Audit
    {
        public string Application { get; set; }
        public string Environment { get; set; }
        public string User { get; set; }
        public string Entity { get; set; }
        public string EntityName { get; set; }
        public AuditAction Action { get; set; }
    }
}