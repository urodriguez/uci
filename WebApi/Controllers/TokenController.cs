using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    [AllowAnonymous]
    public class TokenController : ApiController
    {
        private readonly IUserService _userService;

        public TokenController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] UserLoginDto userLoginDto)
        {
            var token = _userService.Login(userLoginDto);

            return !string.IsNullOrEmpty(token) ? (IHttpActionResult)Ok(token) : Unauthorized();
        }
    }
}