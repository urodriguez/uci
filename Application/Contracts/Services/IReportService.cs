using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IReportService
    {
        Task<IApplicationResult> CreateForProductsAsync(ReportProductDto reportProductDto);
    }
}