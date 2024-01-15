using BooksInventory.API.Models.Domain;
using BooksInventory.API.Models.DTO;
using AutoMapper;

namespace BooksInventory.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<AddBookDTO, Book>().ReverseMap();
            CreateMap<UpdateBookDTO, Book>().ReverseMap();
        } 
    }
}
