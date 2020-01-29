using System.Web.Http;
using Application;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Crosscutting;

namespace WebApi.Controllers
{
    public class TokensController : InventAppApiController
    {
        private readonly IUserService _userService;

        public TokensController(IUserService userService, ILogService logService) : base(logService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] UserLoginDto userLoginDto) => Execute(() => _userService.Login(userLoginDto));
    }
}