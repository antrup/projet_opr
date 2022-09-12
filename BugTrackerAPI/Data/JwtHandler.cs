using BugTrackerAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BugTrackerAPI.Data
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<BugTrackerUser> _userManager;

        public JwtHandler(
            IConfiguration configuration,
            UserManager<BugTrackerUser> userManager
            )
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        // Generate a token based on key and other parameters defined in Secrets file and user data (roles, id, username)
        public async Task<JwtSecurityToken> GetTokenAsync(BugTrackerUser user)
        {
            var jwtOptions = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: await GetClaimsAsync(user),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(
                    _configuration["JwtSettings:ExpirationTimeInMinutes"])),
                signingCredentials: GetSigningCredentials());
            return jwtOptions;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(
                _configuration["JwtSettings:SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret,
                SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaimsAsync(BugTrackerUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
