using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BooksInventory.API.Controllers;
using BooksInventory.API.Models.DTO;
using BooksInventory.API.Repositories;

namespace BooksInventory.Tests.ControllerTests
{   // Test fixture class for testing AuthController methods
    [TestFixture]
    public class AuthControllerTests
    {
        //// Test method to test the Register method with valid input
        [Test]
        public async Task Register_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userManager = MockUserManager.GetMockUserManager();
            var tokenRepository = new Mock<ITokenRepsitory>();

            var controller = new AuthController(userManager.Object, tokenRepository.Object);

            var registerReqDTO = new RegisterReqDTO
            {
                Username = "testuser@google.com",
                Password = "Test@1234",
                Roles = new string[] { "User" }
            };

            userManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(um => um.AddToRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                       .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Register(registerReqDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("User was registered..!", okResult.Value);
        }


        // Test method to test the Login method with valid input
        [Test]
        public async Task Login_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userManager = MockUserManager.GetMockUserManager();
            var tokenRepository = new Mock<ITokenRepsitory>();

            var controller = new AuthController(userManager.Object, tokenRepository.Object);

            var loginReqDTO = new LoginReqDTO
            {
                Username = "testuser@google.com",
                Password = "Test@1234"
            };

            userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(new IdentityUser());

            userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                       .ReturnsAsync(true);

            userManager.Setup(um => um.GetRolesAsync(It.IsAny<IdentityUser>()))
                       .ReturnsAsync(new List<string> { "User" });

            tokenRepository.Setup(tr => tr.CreateJWTToken(It.IsAny<IdentityUser>(), It.IsAny<List<string>>()))
                           .Returns("fakeJwtToken");

            // Act
            var result = await controller.Login(loginReqDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var responseDTO = okResult.Value as LoginResponseDTO;
            Assert.IsNotNull(responseDTO);
            Assert.AreEqual("fakeJwtToken", responseDTO.JwtToken);
        }

        //Test method to test the Login method with invalid input
        [Test]
        public async Task Login_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var userManager = MockUserManager.GetMockUserManager();
            var tokenRepository = new Mock<ITokenRepsitory>();

            var controller = new AuthController(userManager.Object, tokenRepository.Object);

            var loginReqDTO = new LoginReqDTO
            {
                // Invalid input with missing required fields
                Password = "Test@1234"


            };

            // Act
            var result = await controller.Login(loginReqDTO);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Incorrect Username or Password", badRequestResult.Value);
        }
    }

    // To create a mock UserManager
    public static class MockUserManager
    {
        public static Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
