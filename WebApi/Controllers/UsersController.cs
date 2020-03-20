using System;
using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : CrudController<UserDto>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILogService loggerService) : base(userService, loggerService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id:Guid}/confirmEmail")]
        public IHttpActionResult ConfirmEmail([FromUri] Guid id) => Execute(() => _userService.ConfirmEmail(id), MediaType.TextHtml);

        [HttpPatch]
        [Route("{id:Guid}/customPassword")]
        public IHttpActionResult CustomPassword([FromUri] Guid id, [FromBody] PasswordDto passwordDto) => Execute(() => _userService.CustomPassword(id, passwordDto));
    }
}
