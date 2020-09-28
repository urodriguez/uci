using System.Threading.Tasks;
using Application.Contracts;
using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;

namespace Application.Factories
{
    public class InventionFactory : Factory<InventionDto, Invention>, IInventionFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventAppContext _inventAppContext;

        public InventionFactory(IMapper mapper, IUnitOfWork unitOfWork, IInventAppContext inventAppContext) : base(mapper)
        {
            _unitOfWork = unitOfWork;
            _inventAppContext = inventAppContext;
        }

        public override Invention Create(InventionDto dto)
        {
            return new Invention(
                _inventAppContext.UserId,
                dto.Code,
                dto.Name,
                dto.Description,
                dto.CategoryId,
                dto.Price,
                dto.Enable
            );
        }

        public override async Task<InventionDto> CreateAsync(Invention aggregate)
        {
            var dto = await base.CreateAsync(aggregate);//basic mapping

            var inventor = await _unitOfWork.Users.GetByIdAsync(aggregate.UserId);
            dto.InventorName = inventor.GetFullName();            
            
            var category = await _unitOfWork.InventionCategories.GetByIdAsync(dto.CategoryId);
            dto.CategoryName = category.Name;

            return dto;
        }
    }
}