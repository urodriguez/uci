using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.AggregateUpdaters
{
    public interface IUserUpdater : IAggregateUpdater<UserDto, User>
    {
        
    }
}