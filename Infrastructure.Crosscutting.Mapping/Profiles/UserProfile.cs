using Application.Dtos;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.Mapping.Profiles
{
    public class UserProfile : AggregateProfile<UserDto, User>
    {
        public UserProfile()
        {
            //Add more custom mappings
        }
    }
}