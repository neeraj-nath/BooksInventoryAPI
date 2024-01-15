using Microsoft.EntityFrameworkCore;
using BooksInventory.API.Models.Domain;

namespace BooksInventory.API.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
        //DbSet below :
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //To seed data for books:
            var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title= "Ashimat Jar Heraal Seema",                    Author= "Kanchan Baruah",                    ISBN= "978-0-061-99437-7",                    Genre= "Fiction",                    PublicationYear= DateTime.Parse("1945-07-15T14:29:24.945"),                    Price= 550,                    Quantity= 7,                    Description= "A clerk dreaming of a perfect married life with kids, who suddenly gets sucked into a black hole"
                },
                new Book()
                {
                    Id = 2,
                    Title= "To Kill a Mockingbird",                    Author= "Harper Lee",                    ISBN= "978-006112-0084",                    Genre= "Fiction",                    PublicationYear= DateTime.Parse("1960-07-11T00:00:00.000Z"),                    Price= 750,                    Quantity= 17,                    Description= "A classic novel that explores racial injustice and moral growth in the American South during the 1930s."

                },
            };

            modelBuilder.Entity<Book>().HasData(books);
        }
    }




}
