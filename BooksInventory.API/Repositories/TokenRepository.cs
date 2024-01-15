using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BooksInventory.API.Repositories
{
    public class TokenRepository : ITokenRepsitory
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;   
        }

        // Method to create a JWT token for the given user and roles
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            // Create claims 
            var claims = new List<Claim>();

            // Add user's email as a claim
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Add user roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a new JWT token with specified issuer, audience, claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
