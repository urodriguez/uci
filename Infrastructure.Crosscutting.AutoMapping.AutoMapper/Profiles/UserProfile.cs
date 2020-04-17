using Application.Dtos;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.AutoMapping.AutoMapper.Profiles
{
    public class UserProfile : AggregateProfile<UserDto, User>
    {
        public UserProfile()
        {
            //Add more custom mappings
        }
    }
}