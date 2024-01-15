using AutoMapper;
using BooksInventory.API.Controllers;
using BooksInventory.API.Models.Domain;
using BooksInventory.API.Models.DTO;
using BooksInventory.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BooksInventory.Tests.ControllerTests
{
    [TestFixture]
    public class BooksControllerTest
    {
        private IMapper _mapper;

        // Set up AutoMapper configuration 
        [SetUp]
        public void Setup()
        {
            // Set up AutoMapper configuration
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, BookDTO>();
            });

            // Create an instance of IMapper using the configured mapperConfig
            _mapper = new Mapper(mapperConfig);
        }


        //Test GetAll books method :
        [Test]
        public async Task GetAll_ReturnsOkWithBookDTOList()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            var mapperMock = new Mock<IMapper>(); // Replace  IMapper with your actual AutoMapper interface

            var bookController = new BooksController(bookRepositoryMock.Object, mapperMock.Object); // Create an instance of your BookController

            // Sample list of books for testing
            var books = new List<Book>
        {
            new Book 
            { 
                Id = 1, 
                Title = "Book1",
                Author = "John Doe First",
                ISBN = "123-45678-9",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            },
            new Book 
            { 
                Id = 2, 
                Title = "Book2",
                Author = "John Doe Second",
                ISBN = "12-3-4567-89",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            }
        };

            // Sample list of expected BookDTOs
            var expectedBookDTOs = new List<BookDTO>
        {
            new BookDTO 
            {
                Id = 1,
                Title = "Book1",
                Author = "John Doe First",
                ISBN = "123-45678-9",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            },
            new BookDTO 
            {
                Id = 2,
                Title = "Book2",
                Author = "John Doe Second",
                ISBN = "12-3-4567-89",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            }
        };

            // Mock the behavior of the bookRepository to return the sample list of books
            bookRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(books);

            // Mock the behavior of the mapper to map from List<Book> to List<BookDTO>
            mapperMock.Setup(mapper => mapper.Map<List<BookDTO>>(books)).Returns(expectedBookDTOs);

            // Act
            var result = await bookController.GetAll();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            Assert.AreEqual(200, okObjectResult.StatusCode);

            var returnedBookDTOs = okObjectResult.Value as List<BookDTO>;
            Assert.IsNotNull(returnedBookDTOs);

            CollectionAssert.AreEqual(expectedBookDTOs, returnedBookDTOs);
        }

        // Test method to test GetById book method with no books
        [Test]
        public async Task GetAll_WithNoBooks_ReturnsEmptyList()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            var mapperMock = new Mock<IMapper>();
            var bookController = new BooksController(bookRepositoryMock.Object, mapperMock.Object);

            // Mock the behavior of the bookRepository to return an empty list of books
            bookRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Book>());

            // Act
            var result = await bookController.GetAll();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            // Check that the returned value is a non-null list
            var returnedBookDTOs = okObjectResult.Value as List<BookDTO>;
            Assert.IsNull(returnedBookDTOs);
        }


        //Test method to test GetById book method using a valid Id:
        [Test]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            int validId = 1;
            var mockRepository = new Mock<IBookRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BooksController(mockRepository.Object, mockMapper.Object);

            var book = new Book 
            { 
                Id = validId,
                Title = "Sample Book",
                Author = "Author Neeraj",
                ISBN = "1234-567-89",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            };
            var bookDTO = new BookDTO 
            { 
                Id = validId,
                Title = "Sample Book",
                Author = "John Doe",
                ISBN = "123456789",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            };

            mockRepository.Setup(repo => repo.GetById(validId)).ReturnsAsync(book);
            mockMapper.Setup(mapper => mapper.Map<BookDTO>(It.IsAny<Book>())).Returns(bookDTO);

            // Act
            var result = await controller.GetById(validId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBookDTO = okResult.Value as BookDTO;
            Assert.IsNotNull(returnedBookDTO);
            Assert.AreEqual(bookDTO.Id, returnedBookDTO.Id);
            Assert.AreEqual(bookDTO.Title, returnedBookDTO.Title);
        }

        //Test method to test GetById book method using a invalid Id:
        [Test]
        public async Task GetById_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int invalidId = 99;
            var mockRepository = new Mock<IBookRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BooksController(mockRepository.Object, mockMapper.Object);

            mockRepository.Setup(repo => repo.GetById(invalidId)).ReturnsAsync((Book)null);

            // Act
            var result = await controller.GetById(invalidId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        //Test method to test Create Book method using a valid model: 
        [Test]
        public async Task Create_ValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var mockRepository = new Mock<IBookRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BooksController(mockRepository.Object, mockMapper.Object);

            var addBookDTO = new AddBookDTO
            {
                Title = "Sample Book",
                Author = "John Doe",
                ISBN = "123456789",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            };

            var bookDomainModel = new Book
            {
                Id = 1,
                Title = "Sample Book",
                Author = "John Doe",
                ISBN = "123456789",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            };

            var bookDTO = new BookDTO
            {
                Id = 1,
                Title = "Sample Book",
                Author = "John Doe",
                ISBN = "123456789",
                Genre = "Fiction",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 20,
                Quantity = 50,
                Description = "A sample book for testing purposes"
            };

            mockMapper.Setup(mapper => mapper.Map<Book>(It.IsAny<AddBookDTO>())).Returns(bookDomainModel);
            mockRepository.Setup(repo => repo.Create(bookDomainModel)).Returns(Task.FromResult<Book>(null));
            mockMapper.Setup(mapper => mapper.Map<BookDTO>(It.IsAny<Book>())).Returns(bookDTO);

            // Act
            var result = await controller.Create(addBookDTO);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);

            var returnedBookDTO = createdAtActionResult.Value as BookDTO;
            Assert.IsNotNull(returnedBookDTO);
            Assert.AreEqual(bookDTO.Id, returnedBookDTO.Id);
            Assert.AreEqual(bookDTO.Title, returnedBookDTO.Title);
            Assert.AreEqual(bookDTO.Author, returnedBookDTO.Author);
            Assert.AreEqual(bookDTO.ISBN, returnedBookDTO.ISBN);
            Assert.AreEqual(bookDTO.Genre, returnedBookDTO.Genre);
            Assert.AreEqual(bookDTO.PublicationYear, returnedBookDTO.PublicationYear);
            Assert.AreEqual(bookDTO.Price, returnedBookDTO.Price);
            Assert.AreEqual(bookDTO.Quantity, returnedBookDTO.Quantity);
            Assert.AreEqual(bookDTO.Description, returnedBookDTO.Description);
        }

        //Test method to test Create book method using an invalid model:
        [Test]
        public async Task Create_InvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var mockRepository = new Mock<IBookRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BooksController(mockRepository.Object, mockMapper.Object);

            var invalidAddBookDTO = new AddBookDTO(); // This model is invalid as per the validation rules

            controller.ModelState.AddModelError("Title", "Title is required"); // Add model error for testing

            // Act
            var result = await controller.Create(invalidAddBookDTO);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        // Test method to test Update Book method using valid model but invalid Id:
        [Test]
        public async Task Update_ValidModelAndNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 99;
            var mockRepository = new Mock<IBookRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BooksController(mockRepository.Object, mockMapper.Object);

            var updateBookDTO = new UpdateBookDTO
            {
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "Updated ISBN",
                Genre = "Updated Genre",
                PublicationYear = new DateTime(2022, 1, 1),
                Price = 250,
                Quantity = 75,
                Description = "Updated Description"
            };

            mockRepository.Setup(repo => repo.Update(nonExistingId, It.IsAny<Book>())).ReturnsAsync((Book)null);

            // Act
            var result = await controller.Update(nonExistingId, updateBookDTO);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        //Test method to test Delete book method with valid existing Id:
        [Test]
        public async Task Delete_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int existingId = 1;
            var mockRepository = new Mock<IBookRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BooksController(mockRepository.Object, mockMapper.Object);

            var existingBook = new Book
            {
                Id = existingId,
                // Set other properties
            };

            mockRepository.Setup(repo => repo.Delete(existingId)).ReturnsAsync(existingBook);
            mockMapper.Setup(mapper => mapper.Map<BookDTO>(It.IsAny<Book>())).Returns(new BookDTO());

            // Act
            var result = await controller.Delete(existingId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBookDTO = okResult.Value as BookDTO;
            Assert.IsNotNull(returnedBookDTO, "Returned BookDTO is null");
        }

        //Test method to test Delete method of book with Invalid non-exsiting Id:
        [Test]
        public async Task Delete_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 99;
            var mockRepository = new Mock<IBookRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BooksController(mockRepository.Object, mockMapper.Object);

            // Simulate the case where the book with the given ID does not exist
            mockRepository.Setup(repo => repo.Delete(nonExistingId)).ReturnsAsync((Book)null);

            // Act
            var result = await controller.Delete(nonExistingId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }



    }
}