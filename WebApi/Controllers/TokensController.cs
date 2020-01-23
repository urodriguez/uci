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
            var applicationResult = _userService.Login(userLoginDto);

            return applicationResult.Status == 2 ? Unauthorized() : (IHttpActionResult)Ok(applicationResult);
        }
    }
}