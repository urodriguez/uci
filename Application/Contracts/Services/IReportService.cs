using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IReportService
    {
        IApplicationResult CreateForProducts(ReportProductDto reportProductDto);
    }
}