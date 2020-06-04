using System.Threading.Tasks;

namespace Infrastructure.Crosscutting.Reporting
{
    public interface IReportInfrastructureService
    {
        Task<byte[]> CreateAsync(Report report);
    }
}