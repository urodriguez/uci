using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
    public interface IUserFactory : IFactory<UserDto, User>
    {
        
    }
}