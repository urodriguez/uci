using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Auditing;

namespace Application.Services
{
    public class ProductTypeService : CrudService<ProductTypeDto, ProductType>, IProductTypeService
    {
        public ProductTypeService(IProductTypeRepository repository, IProductTypeAdapter adapter, IAuditService auditService) : base(repository, adapter, auditService)
        {
        }
    }
}