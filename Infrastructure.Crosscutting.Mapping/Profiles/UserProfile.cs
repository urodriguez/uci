using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.Mapping.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}