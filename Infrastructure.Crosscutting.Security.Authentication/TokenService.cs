using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using Domain.Contracts.Infrastructure.Crosscutting;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Crosscutting.Security.Authentication
{
    public class TokenService : DelegatingHandler, ITokenService
    {
        public string Generate(string username)
        {
            // create a claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            // describe security token
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Token.Issuer,
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(Token.Expire)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(Token.Secret)), SecurityAlgorithms.HmacSha256Signature)
            };

            // create JWT security token based on descriptor
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);

            // return security token as string
            return tokenHandler.WriteToken(jwtSecurityToken);
        }

        public IToken Validate(string securityToken)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = Token.Issuer,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                LifetimeValidator = LifetimeValidator,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(Token.Secret)),
                ValidateAudience = false
            };

            // Extract and assign to Current Principal Thread
            Thread.CurrentPrincipal = tokenHandler.ValidateToken(securityToken, validationParameters, out var validatedToken);

            return new Domain.Contracts.Infrastructure.Crosscutting.Token
            {
                Issuer = Token.Issuer,
                Subject = Thread.CurrentPrincipal.Identity
            };
        }

        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    HttpStatusCode statusCode;
        //    string token;

        //    // determine whether a jwt exists or not
        //    if (!TryExtractToken(request, out token) || request.RequestUri.AbsoluteUri.Contains("swagger"))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }

        //    try
        //    {
        //        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidIssuer = Token.Issuer,
        //            ValidateIssuerSigningKey = true,
        //            ValidateLifetime = true,
        //            LifetimeValidator = LifetimeValidator,
        //            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(Token.Secret)),
        //            ValidateAudience = false
        //        };

        //        // Extract and assign Current Principal and user
        //        Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
        //        HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    catch (SecurityTokenValidationException stve)
        //    {
        //        statusCode = HttpStatusCode.Unauthorized;
        //    }
        //    catch (Exception e)
        //    {
        //        statusCode = HttpStatusCode.InternalServerError;
        //    }

        //    return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode), cancellationToken);
        //}
        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires == null) return false;

            return DateTime.UtcNow < expires;
        }


        //private static bool TryExtractToken(HttpRequestMessage request, out string token)
        //{
        //    token = null;
        //    IEnumerable<string> authzHeaders;
        //    if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1) return false;

        //    var bearerToken = authzHeaders.ElementAt(0);
        //    token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
        //    return true;
        //}
    }
}
