using Application.Dtos;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.Mapping.Profiles
{
    public class UserProfile : AggregateProfile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>().ForAllMembers(GetConfiguredOptions<UserDto, User>());
        }
    }
}