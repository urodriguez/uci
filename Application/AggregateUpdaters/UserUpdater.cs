using Application.Contracts.AggregateUpdaters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.AggregateUpdaters
{
    public class UserUpdater : IUserUpdater
    {
        public void Update(User inventionCategory, UserDto dto)
        {
            inventionCategory.SetEmail(dto.Email);
            inventionCategory.SetFirstName(dto.FirstName);
            inventionCategory.MiddleName = dto.MiddleName;
            inventionCategory.SetLastName(dto.LastName);
            inventionCategory.SetRole(dto.Role);
            inventionCategory.Active = dto.Active;
        }
    }
}