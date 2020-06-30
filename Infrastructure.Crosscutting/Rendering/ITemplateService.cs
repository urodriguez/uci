using System.Threading.Tasks;

namespace Infrastructure.Crosscutting.Rendering
{
    public interface ITemplateService
    {
        Task<T> RenderAsync<T>(Template template);
    }
}