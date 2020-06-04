namespace Infrastructure.Crosscutting.Auditing
{
    public interface IAuditService
    {
        void AuditAsync(Audit audit);
    }
}