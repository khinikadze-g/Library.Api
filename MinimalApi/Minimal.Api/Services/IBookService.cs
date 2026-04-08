using Minimal.Api.Models;

namespace Minimal.Api.Services
{
    public interface IBookService
    {
        public Task<bool> CreateAsync(Book book);
        public Task<bool?> UpdateAsync(string isbn, Book book);
        public Task<bool?> DeleteAsync(string isbn);
        public Task<List<Book>> GetAllAsync();
        public Task<Book?> GetByIsbnAsync(string Isbn);
    }
}
