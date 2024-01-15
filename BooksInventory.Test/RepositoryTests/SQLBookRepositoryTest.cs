using BooksInventory.API.Data;
using BooksInventory.API.Models.Domain;
using BooksInventory.API.Repositories;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public class SQLBookRepositoryTests
{
    //Test Method for testing Get All method 
    [Test]
    public async Task GetAll_ReturnsListOfBooks()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "GetAll_ReturnsListOfBooks")
            .Options;

        using (var context = new BookDbContext(options))
        {
            var repository = new SQLBookRepository(context);

            // Add some test books to the in-memory database
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Book1",
                    Author = "John Doe Second",
                    ISBN = "12-3-4567-89",
                    Genre = "Fiction",
                    PublicationYear = new DateTime(2022, 8, 1),
                    Price = 280,
                    Quantity = 50,
                    Description = "A sample book 1 for testing purposes"
                },
                new Book
                {
                    Id = 2,
                    Title = "Book2",
                    Author = "John Doe Second",
                    ISBN = "12-878-4567-09",
                    Genre = "Fiction",
                    PublicationYear = new DateTime(2023, 1, 1),
                    Price = 205,
                    Quantity = 50,
                    Description = "A sample book 2 for testing purposes"
                }
            };

            context.Books.AddRange(books);
            context.SaveChanges();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Book>>(result);
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(books, result);
        }
    }

    //Test Method to test GetAll for empty books Inventory/list
    [Test]
    public async Task GetAll_ReturnsEmptyList_WhenNoBooks()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "GetAll_ReturnsEmptyList_WhenNoBooks")
            .Options;

        using (var context = new BookDbContext(options))
        {
            var repository = new SQLBookRepository(context);

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Book>>(result);
            Assert.IsEmpty(result);
        }
    }

    //Test Method to GetById when Book exist:
    [Test]
    public async Task GetById_ReturnsBook_WhenBookExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "GetById_ReturnsBook_WhenBookExists")
            .Options;

        using (var context = new BookDbContext(options))
        {
            var repository = new SQLBookRepository(context);

            var book = new Book
            {
                Id = 1,
                Title = "Book1",
                Author = "John Doe Second",
                ISBN = "12-3-4567-89",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 8, 1),
                Price = 280,
                Quantity = 50,
                Description = "A sample book 1 for testing purposes"
            };
            context.Books.Add(book);
            context.SaveChanges();

            // Act
            var result = await repository.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Book>(result);
            Assert.AreEqual(book, result);
        }
    }


    // Test Method to GetById when book does not exist:
    [Test]
    public async Task GetById_ReturnsNull_WhenBookDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "GetById_ReturnsNull_WhenBookDoesNotExist")
            .Options;

        using (var context = new BookDbContext(options))
        {
            var repository = new SQLBookRepository(context);

            // Act
            var result = await repository.GetById(1);

            // Assert
            Assert.IsNull(result);
        }
    }
}
