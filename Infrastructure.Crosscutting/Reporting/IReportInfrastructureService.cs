namespace Infrastructure.Crosscutting.Reporting
{
    public interface IReportInfrastructureService
    {
        byte[] Create(Report report);
    }
}