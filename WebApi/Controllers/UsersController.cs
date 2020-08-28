using System;
using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/users")]
    public class UsersController : CrudController<UserDto>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILogService loggerService, IInventAppContext inventAppContext) : base(userService, loggerService, inventAppContext)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetAsync([FromUri] string id) => await ExecuteAsync(
            async () => id.Equals("logged") ? await _userService.GetLoggedAsync() : await _userService.GetByIdAsync(Guid.Parse(id))
        );

        [HttpPut]
        [Route("{id}")]
        public new async Task<IHttpActionResult> UpdateAsync([FromUri] Guid id, [FromBody] UserDto userDto) => await base.UpdateAsync(id, userDto);

        [HttpDelete]
        [Route("{id}")]
        public new async Task<IHttpActionResult> DeleteAsync([FromUri] Guid id) => await base.DeleteAsync(id);      
        
        [HttpGet]
        [Route("{id:Guid}/confirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmailAsync([FromUri] Guid id) => await ExecuteAsync(
            async () => await _userService.ConfirmEmailAsync(id), 
            MediaType.TextHtml
        );

        [HttpPatch]
        [Route("logged/resetPassword")]
        public async Task<IHttpActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto) => await ExecuteAsync(
            async () => await _userService.ResetPasswordAsync(resetPasswordDto)
        );

        [HttpPatch]
        [Route("{email}/forgotPassword")]
        public async Task<IHttpActionResult> ForgotPasswordAsync([FromUri] string email) => await ExecuteAsync(
            async () => await _userService.ForgotPasswordAsync(email)
        );
    }
}
