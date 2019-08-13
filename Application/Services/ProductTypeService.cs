using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.Services
{
    public class ProductTypeService : CrudService<ProductTypeDto, ProductType>, IProductTypeService
    {
        public ProductTypeService(IProductTypeRepository repository, IProductTypeAdapter adapter) : base(repository, adapter)
        {
        }
    }
}