using BooksInventory.API.Models.Domain;

namespace BooksInventory.API.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAll();

        Task<Book?> GetById(int id);

        Task<Book> Create(Book book);

        Task<Book?> Update(int Id,Book book);

        Task<Book?> Delete(int Id);
    }
}
