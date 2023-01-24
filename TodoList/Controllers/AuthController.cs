using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Models;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IJwtSettings _jwtSettings;

        // Hard-coded users (with only username and id)
        private readonly List<UserLite> _users = new()
        {
            new UserLite() { Id = "63c9a265be04a9eae9e0fb01", Username = "Batman"},
            new UserLite() { Id = "63c9a265be04a9eae9e0fb02", Username = "Ironman"},
            new UserLite() { Id = "63c9a265be04a9eae9e0fb03", Username = "Spiderman"},
            new UserLite() { Id = "63c9a265be04a9eae9e0fb04", Username = "Superman"}
        };

        public AuthController(ILogger<AuthController> logger, IJwtSettings jwtSettings)
        {
            _logger = logger;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        public IActionResult Authenticate([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized();
            }

            var userFound = _users.Where(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault();

            if (userFound != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFound?.Id)
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }
    }
}