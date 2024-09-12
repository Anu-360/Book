using Book_Coding_Challenge.Models;
namespace Book_Coding_Challenge.Repository
{
    public interface IBook
    {
        Task<List<Book>> GetAllBooks();
        Task<Book?> GetBookByISBN(string isbn);
        Task AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task DeleteBook(string isbn);
        Task Save();
    }
}
