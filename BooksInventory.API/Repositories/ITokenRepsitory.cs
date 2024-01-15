using Microsoft.AspNetCore.Identity;

namespace BooksInventory.API.Repositories
{
    public interface ITokenRepsitory
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
