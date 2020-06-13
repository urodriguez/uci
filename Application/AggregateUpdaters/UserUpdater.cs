using Application.Contracts.AggregateUpdaters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.AggregateUpdaters
{
    public class UserUpdater : IUserUpdater
    {
        public void Update(User user, UserDto dto)
        {
            user.SetName(dto.Name);
            user.SetEmail(dto.Email);
            user.SetFirstName(dto.FirstName);
            user.MiddleName = dto.MiddleName;
            user.SetLastName(dto.LastName);
            user.SetRole(dto.Role);
            user.Activate = dto.Activate;
        }
    }
}