using books_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace books_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book()
                {
                    Id = 1,
                    Name = "Such A Fun Age",
                    Author = "Kiley Reid"
                },
                new Book()
                {
                    Id = 2,
                    Name = "Temporada de Huracanes",
                    Author = "Fernanda Melchor"
                });
        }
    }
}
