using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Persistence.Repositories
{
    public class ProductTypeRepository: Repository<ProductType>, IProductTypeRepository
    {
        
    }
}