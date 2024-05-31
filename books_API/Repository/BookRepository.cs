using books_API.Data;
using books_API.Models;
using books_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace books_API.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;
        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Book book)
        {
            _db.Add(book);
        }

        public async Task<ICollection<Book>> GetAllAsync()
        {
            return await _db.Books.ToListAsync();
        }

        public async Task<Book> GetAsync(int id)
        {
            return await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Book> GetAsync(string couponName)
        {
            return await _db.Books.FirstOrDefaultAsync(u => u.Name.ToLower() == couponName.ToLower());
        }


        public async Task RemoveAsync(Book book)
        {
            _db.Books.Remove(book);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
