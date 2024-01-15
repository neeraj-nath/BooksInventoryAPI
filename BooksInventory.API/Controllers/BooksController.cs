using AutoMapper;
using BooksInventory.API.Data;
using BooksInventory.API.Models.Domain;
using BooksInventory.API.Models.DTO;
using BooksInventory.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //private readonly BookDbContext dbContext;
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;
        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            //this.dbContext = dbContext; 
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }

        // To get all the books from the Inventory:
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAll()
        {
            //fetch all books:
            var books = await bookRepository.GetAll();

            //Convert books to booksDTO using automapper and returning Ok
            return Ok(mapper.Map<List<BookDTO>>(books));
        }

        //To get a book using its Id:
        [HttpGet]
        [Route("{Id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById(int Id) 
        {
            var bookById = await bookRepository.GetById(Id);
            if (bookById == null) { return NotFound(); }

            return Ok(mapper.Map<BookDTO>(bookById));
        }

        //To add a new book to the Inventory:
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([FromBody]AddBookDTO addBookDTO)
        {
            if (ModelState.IsValid)
            {
                //Convert AddBookDTO to Book model :
                var bookDomainModel = mapper.Map<Book>(addBookDTO);

                await bookRepository.Create(bookDomainModel);

                //Map Book model to bookDTO
                var bookDTO = mapper.Map<BookDTO>(bookDomainModel);
                return CreatedAtAction(nameof(GetById), new { Id = bookDTO.Id }, bookDTO);
            }
            return BadRequest(ModelState);
        }


        // To Update a book in the Book Inventory:
        [HttpPut]
        [Route("{Id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute]int Id, [FromBody]UpdateBookDTO book)
        {
            if (ModelState.IsValid)
            {
                //Convert the BookDTO to book :
                var bookDomainModel = mapper.Map<Book>(book);

                // Check whether with the given Id exists or not:
                bookDomainModel = await bookRepository.Update(Id, bookDomainModel);

                //If the book does not exist:
                if (bookDomainModel == null) { return NotFound(); }


                // Convert book domain model to bookdto model for returning:
                return Ok(mapper.Map<BookDTO>(bookDomainModel));
            }
            return BadRequest(ModelState);
        }

        //To delete a book from the inventory :
        [HttpDelete]
        [Route("{Id:int}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete([FromRoute]int Id)
        {
            // First check whether any book with that id exists:
            var bookDomainModel = await bookRepository.Delete(Id);

            // If the book does not exist:
            if (bookDomainModel == null) { return NotFound(); }

            
            //If book exists, convert the book domain model to bookDTO to return:
            return Ok(mapper.Map<BookDTO>(bookDomainModel));

        }

    }
}
