
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Options;
using Microsoft.Extensions.Options;


namespace CommonLayer.SecurityHelper
{
    public class SecurityHelper : ISecurityHelper
    {
        private readonly JwtOptions _jwt;

        public SecurityHelper(IOptions<JwtOptions> jwt)
        {
            _jwt = jwt.Value;
        }

        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }

        public string GenerateDefaultPassword(string name)
        {
            var firstName = (name ?? string.Empty)
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault() ?? "User";

            var clean = new string(firstName.Where(char.IsLetterOrDigit).ToArray());

            if (clean.Length == 0) clean = "User";
            if (clean.Length < 4) clean = clean.PadRight(4, 'x');   // keeps it at 8+ characters

            clean = char.ToUpperInvariant(clean[0]) + clean[1..].ToLowerInvariant();

            return $"{clean}@123";
        }

        public string GenerateJwtToken(int userId, string username, string role)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
