using Application.Contracts.Adapters;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Adapters
{
    public class UserAdapter : Adapter<UserDto, User>, IUserAdapter
    {
        public UserAdapter(IMapper mapper) : base(mapper)
        {
        }
    }
}