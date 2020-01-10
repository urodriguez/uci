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

        //public override User Create(UserDto userDto, User existingUser = default(User))
        //{
        //    var user = existingUser != null ? _mapper.Map<UserDto, User>(userDto, existingUser) : _mapper.Map<UserDto, User>(userDto);

        //    user.Id = Guid.NewGuid();

        //    return user;
        //}

        //protected override void CompleteInternalFields(User user)
        //{
        //    user.DateCreated = DateTime.Now;
        //}
    }
}