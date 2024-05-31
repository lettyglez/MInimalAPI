using AutoMapper;
using books_API.Models;
using books_API.Models.DTO;

namespace books_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig() { 
            CreateMap<Book, BookCreateDTO>().ReverseMap();
        }
    }
}
