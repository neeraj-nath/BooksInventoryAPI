﻿namespace BooksInventory.API.Models.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public DateTime PublicationYear { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}
