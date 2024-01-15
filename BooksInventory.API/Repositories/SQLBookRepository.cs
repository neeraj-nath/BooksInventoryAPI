using BooksInventory.API.Data;
using BooksInventory.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BooksInventory.API.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly BookDbContext dbContext;
        public SQLBookRepository(BookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Book>> GetAll()
        {
            return await dbContext.Books.ToListAsync();
        }
        public async Task<Book?> GetById(int Id)
        {
            // check for the book with the given id
            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == Id);

            //if book not found
            if (book == null) { return null; }

            //if book found
            return book;
        }

        public async Task<Book> Create(Book book)
        {
            //Add the book model to the database
            await dbContext.Books.AddAsync(book);

            //Save the changes to the database
            await dbContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> Update(int Id, Book book)
        {
            // check the book with the id
            var bookToBeUpdated = await dbContext.Books.FirstOrDefaultAsync(a => a.Id == Id);

            //if book not found
            if (bookToBeUpdated == null) { return null; }
            
            // if book found make the necessary updates
            bookToBeUpdated.Title = book.Title;
            bookToBeUpdated.Author = book.Author;
            bookToBeUpdated.ISBN = book.ISBN;
            bookToBeUpdated.Genre = book.Genre;
            bookToBeUpdated.PublicationYear = book.PublicationYear;
            bookToBeUpdated.Price = book.Price;
            bookToBeUpdated.Quantity = book.Quantity;
            bookToBeUpdated.Description = book.Description;

            //Save changes
            await dbContext.SaveChangesAsync();

            return bookToBeUpdated;
        }

        public async Task<Book?> Delete(int Id)
        {
            // Check the book with that id:
            var bookToBeDeleted = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == Id);

            // if book not found :
            if (bookToBeDeleted == null) { return null; }

            //if book found
            dbContext.Books.Remove(bookToBeDeleted);

            //Save changes
            await dbContext.SaveChangesAsync();
            return bookToBeDeleted;
        }
    }
}
