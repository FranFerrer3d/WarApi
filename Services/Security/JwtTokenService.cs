using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WarApi.Models;

namespace WarApi.Services.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(Player player)
        {
            var handler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(_config["JWT_SECRET"]);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, player.ID.ToString()),
                    new Claim(ClaimTypes.Role, player.Rol.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
