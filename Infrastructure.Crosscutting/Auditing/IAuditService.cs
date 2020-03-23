namespace Infrastructure.Crosscutting.Auditing
{
    public interface IAuditService
    {
        void Audit(Audit audit);
    }
}