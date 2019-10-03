using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Adapters
{
    public interface IUserAdapter : IAdapter<UserDto, User>
    {
        
    }
}