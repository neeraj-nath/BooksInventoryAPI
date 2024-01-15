using BooksInventory.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[TestFixture]
public class TokenRepositoryTests
{
    //Test method to Test for valid JWT token
    [Test]
    public void CreateJWTToken_GeneratesValidToken()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c["Jwt:Issuer"]).Returns("testIssuer");
        configuration.Setup(c => c["Jwt:Audience"]).Returns("testAudience");
        configuration.Setup(c => c["Jwt:Key"]).Returns("abcdefghijklmnopqrstuvwxyzaabbccddeeffgghhiijjkkllmmnnooppqqrrssttuuvvwwxxyyzz11223344556677889900");

        var tokenRepository = new TokenRepository(configuration.Object);

        var user = new IdentityUser
        {
            Email = "testuser@example.com",
        };

        var roles = new List<string> { "User", "Admin" };

        // Act
        var jwtToken = tokenRepository.CreateJWTToken(user, roles);

        // Assert
        Assert.IsNotNull(jwtToken);

        // Validate the JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Object["Jwt:Key"]));
        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration.Object["Jwt:Issuer"],
            ValidAudience = configuration.Object["Jwt:Audience"],
            IssuerSigningKey = key,
        };

        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);

        Assert.IsNotNull(validatedToken);
        Assert.IsNotNull(principal);

        var claimsIdentity = principal.Identity as ClaimsIdentity;
        Assert.IsNotNull(claimsIdentity);

        // Validate the claims
        Assert.AreEqual("testuser@example.com", claimsIdentity.FindFirst(ClaimTypes.Email)?.Value);

        // Validate the roles
        Assert.IsTrue(claimsIdentity.HasClaim(ClaimTypes.Role, "User"));
        Assert.IsTrue(claimsIdentity.HasClaim(ClaimTypes.Role, "Admin"));
    }

    //Test method to test for Invalid JWT token
    [Test]
    public void CreateJWTToken_WithEmptyRoles_ReturnsValidToken()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c["Jwt:Issuer"]).Returns("testIssuer");
        configuration.Setup(c => c["Jwt:Audience"]).Returns("testAudience");
        configuration.Setup(c => c["Jwt:Key"]).Returns("abcdefghijklmnopqrstuvwxyzaabbccddeeffgghhiijjkkllmmnnooppqqrrssttuuvvwwxxyyzz11223344556677889900");

        var tokenRepository = new TokenRepository(configuration.Object);

        var user = new IdentityUser
        {
            Email = "testuser@example.com",
        };

        var roles = new List<string> { "User", "Admin" };

        // Act
        var jwtToken = tokenRepository.CreateJWTToken(user, roles);

        // Assert
        Assert.IsNotNull(jwtToken);
        Assert.IsTrue(jwtToken.Length > 0);
    }
}
