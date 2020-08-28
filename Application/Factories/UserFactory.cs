using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Factories
{
    public class UserFactory : Factory<UserDto, User>, IUserFactory
    {
        public UserFactory(IMapper mapper) : base(mapper)
        {
        }

        public override User Create(UserDto dto)
        {
            return new User(
                dto.Email,
                dto.FirstName,
                dto.MiddleName,
                dto.LastName,
                dto.Role
            );
        }
    }
}