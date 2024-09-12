using Moq;
using NuGet.ContentModel;
using Book_Coding_Challenge.Repository;
using Book_Coding_Challenge.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookTests
{
    public class BookTests
    {
        private IBook book;
        private Mock<IBook> bookMock;


        [SetUp]
        public void SetUp()
        {
            bookMock = new Mock<IBook>();
            book = bookMock.Object;
        }

        [TestCase]
        public async Task ReturnsAllBooksAsync()
        {

            // Arrange
            var expectedBooks = new List<Book>
                {
                new Book { ISBN = "ES-78",Author="Charles", Publication_Year=2002,Title="Tale of Two Cities"},
                new Book { ISBN = "ADFG-90", Author="Helena",Publication_Year=1986,Title="Eagle"}
                };

            // Mock the repository methods
            bookMock.Setup(a => a.GetAllBooks())
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await book.GetAllBooks();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(2, result.Count, "Book count should be 2");
            Assert.AreEqual("Eagle", result[1].Title);
            Assert.AreEqual("Charles", result[0].Author);
        }

        [TestCase]
        public async Task AddBook_ShouldAddBook()
        {
            // Arrange
            var newBook = new Book { ISBN = "THJKN-89", Author = "Agatha", Title = "Then There Were None", Publication_Year = 1956 };


            // Mock
            bookMock.Setup(repo => repo.AddBook(It.IsAny<Book>())).Callback((Book book) => { });

            // Act
            await book.AddBook(newBook);

            //Assert
            bookMock.Verify(repo => repo.AddBook(It.Is<Book>(b => b.ISBN == newBook.ISBN && b.Author == newBook.Author)), Times.Once);
        }

        [TestCase]

        public async Task Save_ShouldCallSaveChanges()
        {
            // Act
            await book.Save();

            // Assert
            bookMock.Verify(repo => repo.Save(), Times.Once);
        }

        [TestCase]
        public async Task DeleteBook_ShouldRemoveBook()
        {
            // Arrange
            var ISBNToDelete = "ES-78";

            // Mock
            bookMock.Setup(repo => repo.DeleteBook(It.IsAny<string>())).Callback<string>(isbn => { });


            // Act
            await book.DeleteBook(ISBNToDelete);

            // Assert
            bookMock.Verify(repo => repo.DeleteBook(It.Is<string>(isbn => isbn == ISBNToDelete)), Times.Once);
        }

        [TestCase]
        public async Task UpdateBook_ShouldUpdateBook()
        {
            // Arrange
            var updatedBook = new Book { ISBN = "TH-90", Author = "Henry", Title = "Divine World", Publication_Year = 2006 };

            // Mock
            bookMock.Setup(repo => repo.UpdateBook(It.IsAny<Book>())).ReturnsAsync((Book set) => set);

            // Act
            var result = await book.UpdateBook(updatedBook);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(updatedBook.ISBN, result.ISBN, "ISBN should match");
            Assert.AreEqual(updatedBook.Author, result.Author, "Author name should be updated");

            bookMock.Verify(repo => repo.UpdateBook(It.Is<Book>(b =>
                b.ISBN == updatedBook.ISBN && b.Author == updatedBook.Author)), Times.Once);
        }
    }


    }