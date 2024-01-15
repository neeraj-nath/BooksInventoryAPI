using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksInventory.API.Data
{
    public class BooksAuthDbContext : IdentityDbContext
    {
        public BooksAuthDbContext(DbContextOptions<BooksAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var userId = "b40f7ec8-1aec-4122-b0a4-9add0a4837b9";
            var AdminId = "60c3e5b6-bf05-4cc8-973a-80af6edad79d";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userId,
                    ConcurrencyStamp = userId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                },
                new IdentityRole
                {
                    Id = AdminId,
                    ConcurrencyStamp = AdminId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),

                }

            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
