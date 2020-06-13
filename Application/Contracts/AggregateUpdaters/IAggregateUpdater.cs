using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts.AggregateUpdaters
{
    public interface IAggregateUpdater<TDto, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
        where TDto : ICrudDto
    {
        void Update(TAggregateRoot aggregateRoot, TDto dto);
    }
}