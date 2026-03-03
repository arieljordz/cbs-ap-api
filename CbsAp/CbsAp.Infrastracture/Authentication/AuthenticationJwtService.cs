using CbsAp.Application.Abstractions.Services.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CbsAp.Infrastracture.Authentication
{
    public class AuthenticationJwtService : IAuthenticationJwtService
    {
        private readonly ILogger<AuthenticationJwtService> _logger;
        private readonly JWTSettings _jwtSettings;

        public AuthenticationJwtService(IOptions<JWTSettings> jwtSettings, ILogger<AuthenticationJwtService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public string GenerateUserJwtToken(string username,string role,decimal authorisationLimit, string[] permissions)
        {
            //
            var secretKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var signingCredentials =
                new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //TODO CLAIMS -- need to configure with enrich data
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, username.Trim()),
                new Claim(JwtRegisteredClaimNames.Sub, username.Trim()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("authorisationlimit", authorisationLimit.ToString()),
            };
            // need to incorporate for the user roles in claim

            foreach(var permission in permissions)
            {
                claims.Add(new Claim("permission",permission));
            }

            var securityToken = new JwtSecurityToken(
                  issuer: _jwtSettings.Issuer,
                  audience: _jwtSettings.Audience,
                  expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                  claims: claims,
                  signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}