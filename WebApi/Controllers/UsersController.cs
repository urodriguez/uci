using System;
using System.Web.Http;
using Application.ApplicationResults;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Crosscutting;

namespace WebApi.Controllers
{
    public class UsersController : CrudController<UserDto>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILogService loggerService) : base(userService, loggerService)
        {
            _userService = userService;
        }

        [HttpPatch]
        public IHttpActionResult ConfirmEmail([FromUri] Guid id)
        {
            return Execute(() =>
            {
                _userService.ConfirmEmail(id);
                return new EmptyResult();
            });
        }
    }
}
