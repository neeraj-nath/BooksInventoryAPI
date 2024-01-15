using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BooksInventory.API.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace BooksInventory.Tests.DataTests
{
    [TestFixture]
    public class BooksAuthDbContextTests
    {
        // Test Method to see that roles are seeded successfully in the DbContext
        [Test]
        public void OnModelCreating_SeedRoles_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BooksAuthDbContext>()
                .UseInMemoryDatabase(databaseName: "OnModelCreating_SeedRoles_Success")
                .Options;

            using (var context = new BooksAuthDbContext(options))
            {
                // Act
                context.Database.EnsureCreated();

                // Assert
                // Checking if 'User' role is seeded successfully
                var userRole = context.Roles.SingleOrDefault(r => r.Name == "User");
                Assert.IsNotNull(userRole);
                Assert.AreEqual("USER", userRole.NormalizedName);

                //Checking if 'Admin' role is seeded successfully
                var adminRole = context.Roles.SingleOrDefault(r => r.Name == "Admin");
                Assert.IsNotNull(adminRole);
                Assert.AreEqual("ADMIN", adminRole.NormalizedName);
            }
        }

        // Test case to add a new role to the DbContext
        [Test]
        public void OnModelCreating_AddNewRole_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BooksAuthDbContext>()
                .UseInMemoryDatabase(databaseName: "OnModelCreating_AddNewRole_Success")
                .Options;

            // Act & Assert
            using (var context = new BooksAuthDbContext(options))
            {
                // Seed roles
                context.Database.EnsureCreated();

                // Assert existing roles
                var userRole = context.Roles.SingleOrDefault(r => r.Name == "User");
                Assert.IsNotNull(userRole);
                Assert.AreEqual("USER", userRole.NormalizedName);

                var adminRole = context.Roles.SingleOrDefault(r => r.Name == "Admin");
                Assert.IsNotNull(adminRole);
                Assert.AreEqual("ADMIN", adminRole.NormalizedName);

                // Add a new role
                var newRole = new IdentityRole
                {
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                };

                context.Roles.Add(newRole);
                context.SaveChanges();

                // Assert the newly added role
                var addedRole = context.Roles.SingleOrDefault(r => r.Name == "Moderator");
                Assert.IsNotNull(addedRole);
                Assert.AreEqual("MODERATOR", addedRole.NormalizedName);
            }
        }

    }
}
