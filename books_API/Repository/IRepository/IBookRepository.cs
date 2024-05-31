using books_API.Models;

namespace books_API.Repository.IRepository
{
    public interface IBookRepository
    {
        Task<ICollection<Book>> GetAllAsync();
        Task<Book> GetAsync(int id);
        Task<Book> GetAsync(string bookName);
        Task CreateAsync(Book book);
        Task RemoveAsync(Book book);
        Task SaveAsync();
    }
}
