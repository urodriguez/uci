using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    public class TokensController : ApiController
    {
        private readonly IUserService _userService;

        public TokensController(IUserService userService)
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