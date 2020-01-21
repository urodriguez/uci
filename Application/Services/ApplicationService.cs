using System;
using Domain.Contracts.Infrastructure.Crosscutting;

namespace Application.Services
{
    public abstract class ApplicationService
    {
        private readonly ITokenService _tokenService;

        protected ApplicationService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected void CheckAuthorization()
        {
            var validatedToken = _tokenService.Validate(InventAppContext.SecurityToken);
            InventAppContext.UserName = validatedToken.Subject.Name;
        }
    }
}