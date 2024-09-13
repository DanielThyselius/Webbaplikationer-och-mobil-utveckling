using DemoModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace Demo.Api
{
    public class BookService : IBookService
    {
        List<Book> _books;

        public BookService()
        {
            //_context = context;
            _books = Book.GetSampleData();
        }

        public List<Book> GetBooks()
        {
            return _books;
        }

        public Book GetBookById(int id)
        {
            //return _context.Books.FirstOrDefault(b => b.Id == id);
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            book.Id = _books.Max(b => b.Id) + 1;
            _books.Add(book);
        }

        public void Update(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);

            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Review = book.Review;
                existingBook.Description = book.Description;
            }
        }

        public void Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                _books.Remove(book);
            }
        }
    }
}
