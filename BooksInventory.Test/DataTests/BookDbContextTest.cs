using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using BooksInventory.API.Data;
using BooksInventory.API.Models.Domain;
using Microsoft.EntityFrameworkCore.InMemory;

namespace BooksInventory.Tests.DataTests
{
    [TestFixture]
    public class BookDbContextTests
    {
        // Test to check if the DbContext can be instantiated with the provided DbContextOptions
        [Test]
        public void DbContext_CanBeInstantiated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Act
            using (var context = new BookDbContext(options))
            {
                // Assert
                Assert.IsNotNull(context);
            }
        }

        // Test to check if the DbContext can seed data for books
        [Test]
        public void DbContext_SeedDataForBooks()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Act
            using (var context = new BookDbContext(options))
            {
                // Seed data for books
                context.Database.EnsureCreated();

                // Assert
                Assert.AreEqual(2, context.Books.Count()); // Check if there are 2 books in the database

                // Check if the first book is seeded correctly
                var firstBook = context.Books.First();
                Assert.AreEqual(1, firstBook.Id);
                Assert.AreEqual("Ashimat Jar Heraal Seema", firstBook.Title);
                Assert.AreEqual("Kanchan Baruah", firstBook.Author);
                Assert.AreEqual("978-0-061-99437-7", firstBook.ISBN);
                Assert.AreEqual("Fiction", firstBook.Genre);
                Assert.AreEqual(DateTime.Parse("1945-07-15T14:29:24.945"), firstBook.PublicationYear);
                Assert.AreEqual(550, firstBook.Price);
                Assert.AreEqual(7, firstBook.Quantity);
                Assert.AreEqual("A clerk dreaming of a perfect married life with kids, who suddenly gets sucked into a black hole", firstBook.Description);
            }
        }
    }
}
