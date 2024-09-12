using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Book_Coding_Challenge.Models;
using Book_Coding_Challenge.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Book_Coding_Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;
        private readonly IBook _book;

        public BooksController(BookContext context,IBook book)
        {
            _context = context;
            _book = book;
        }

        // GET: api/Books
        [HttpGet]
        [Authorize(Roles ="Admin,Reader")]
      
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _book.GetAllBooks();
        }

        // GET: api/Books/5
        [HttpGet("{isbn}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Book>> GetBook(string isbn)
        {
            var book = await _book.GetBookByISBN(isbn);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{isbn}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> PutBook(string isbn, Book book)
        {
            if (isbn != book.ISBN)
            {
                return BadRequest();
            }

            await _book.UpdateBook(book);

            try
            {
                await _book.Save();
                _book.UpdateBook(book);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (! BookExists(isbn))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _book.AddBook(book);
            await _book.Save(); 

            return CreatedAtAction(nameof(GetBook), new { isbn = book.ISBN }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{isbn}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            try
            {
                await _book.DeleteBook(isbn);
                await _book.Save();
                return NoContent();
            }
            catch (Exception)
            {
                if (isbn == null)
                    return NotFound();
                return BadRequest();
            }
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.ISBN == id);
        }
    }
}
