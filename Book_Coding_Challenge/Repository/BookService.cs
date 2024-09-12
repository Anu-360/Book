using Microsoft.EntityFrameworkCore;
using Book_Coding_Challenge.Models;
using Book_Coding_Challenge.Exceptions;

namespace Book_Coding_Challenge.Repository
{
    public class BookService:IBook
    {
        private readonly BookContext _context;

        public BookService(BookContext context)
        {
            _context = context;
        }
        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByISBN(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task AddBook(Book book)
        {
            _context.Books.Add(book);
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _context.Books.Update(book);
            return book;
        }

        public async Task DeleteBook(string isbn)
        {
            var asset = await _context.Books.FindAsync(isbn);
            if (asset == null)
            {
                throw new BookNotFoundException($"Book with ISBN {isbn} Not Found");
            }

            _context.Books.Remove(asset);

        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
