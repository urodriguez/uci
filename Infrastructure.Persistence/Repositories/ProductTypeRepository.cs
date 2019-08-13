using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductTypeRepository: Repository<ProductType>, IProductTypeRepository
    {
        
    }
}