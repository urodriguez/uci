using System.Threading.Tasks;
using Application.Infrastructure.Redering;

namespace Application.Contracts.Infrastructure.Redering
{
    public interface ITemplateService
    {
        Task<T> RenderAsync<T>(Template template);
    }
}