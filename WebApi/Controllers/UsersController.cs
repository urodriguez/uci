using System;
using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : CrudController<UserDto>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILogService loggerService, IInventAppContext inventAppContext) : base(userService, loggerService, inventAppContext)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id:Guid}/confirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmailAsync([FromUri] Guid id) => await ExecuteAsync(async () => await _userService.ConfirmEmailAsync(id), MediaType.TextHtml);

        [HttpPatch]
        [Route("{id:Guid}/customPassword")]
        public async Task<IHttpActionResult> CustomPasswordAsync([FromUri] Guid id, [FromBody] PasswordDto passwordDto) => await ExecuteAsync(async () => await _userService.CustomPasswordAsync(id, passwordDto));

        [HttpGet]
        [Route("{userName}/forgotPassword")]
        public async Task<IHttpActionResult> ForgotPasswordAsync([FromUri] string userName) => await ExecuteAsync(async () => await _userService.ForgotPasswordAsync(userName));
    }
}
