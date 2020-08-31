using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    public class TokensController : InventAppApiController
    {
        private readonly IUserService _userService;

        public TokensController(IUserService userService, ILogService logService, IInventAppContext inventAppContext) : base(logService, inventAppContext)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync([FromBody] UserCredentialDto userCredential) => await ExecuteAsync(
            async () => await _userService.LoginAsync(userCredential)
        );
    }
}