using BooksInventory.API.Models.DTO;
using BooksInventory.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BooksInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenRepsitory tokenRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepsitory tokenRepository)
        {
            this.tokenRepository = tokenRepository;
            this.userManager = userManager;
        }
        //Implement Post Method for Registering User:
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterReqDTO registerReqDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerReqDTO.Username,
                Email = registerReqDTO.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerReqDTO.Password);

            if (identityResult.Succeeded)
            {
                //Add roles to the User :
                if (registerReqDTO.Roles != null && registerReqDTO.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerReqDTO.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("User was registered..!");
                    }
                }
            }
            return BadRequest("There seems to be an error");

        }
        //Implement Post Method for Logging in the User:
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginReqDTO loginReqDTO)
        {
            var user = await userManager.FindByEmailAsync(loginReqDTO.Username);

            if(user != null) 
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, loginReqDTO.Password);

                if (checkPassword)
                {
                    //Get roles for the respective User :
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //Create a Token:
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(response);
                    }
                }
            }

            return BadRequest("Incorrect Username or Password");
        }
    }
}
