﻿using Microsoft.EntityFrameworkCore;
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
                    Title= "Ashimat Jar Heraal Seema",
                },
                new Book()
                {
                    Id = 2,
                    Title= "To Kill a Mockingbird",

                },
            };

            modelBuilder.Entity<Book>().HasData(books);
        }
    }




}