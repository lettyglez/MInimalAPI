using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using books_API.Data;
using books_API.Models;

namespace books_API.Repository.Tests
{
    [TestClass()]
    public class BookRepositoryTests
    {
        [TestMethod()]
        public async Task GetAllAsyncTest()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "BookListDatabase")
           .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Books.Add(new Models.Book { Id = 1, Name = "Bajar es lo Peor", Author = "Mariana Enriquez" });
                context.Books.Add(new Models.Book { Id = 2, Name = "Nuestra Parte de Noche", Author = "Mariana Enriquez" });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                BookRepository bookRepository = new BookRepository(context);
                var bookList = await bookRepository.GetAllAsync();

                Assert.AreEqual(2, bookList.Count);
            }
        }

        [TestMethod()]
        public async Task CreateAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "BookListDatabase")
            .Options;

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                BookRepository bookRepository = new BookRepository(context);

                Book book = new Book{
                    Name = "Desde los Zulos",
                    Author = "Dahlia de la Cerda"
                };
                
                await bookRepository.CreateAsync(book);
                await bookRepository.SaveAsync();

            }
        }
    }
}
