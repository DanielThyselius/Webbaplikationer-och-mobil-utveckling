using DemoModels;

namespace Demo.Api
{
    public interface IBookService
    {
        Book GetBookById(int id);
        List<Book> GetBooks();

        void Add(Book book);
        void Update(Book book);
        void Delete(int id);
    }
}