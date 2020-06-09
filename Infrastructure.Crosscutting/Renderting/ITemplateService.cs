using System.Threading.Tasks;

namespace Infrastructure.Crosscutting.Renderting
{
    public interface ITemplateService
    {
        Task<T> RenderAsync<T>(Template template);
    }
}