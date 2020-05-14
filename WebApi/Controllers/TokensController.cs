using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    public class TokensController : InventAppApiController
    {
        private readonly IUserService _userService;

        public TokensController(IUserService userService, ILogService logService, IAppSettingsService appSettingsService) : base(logService, appSettingsService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CredentialsDto credentialsDto) => Execute(() => _userService.Login(credentialsDto));
    }
}