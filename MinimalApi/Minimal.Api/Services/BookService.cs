using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minimal.Api.Data;
using Minimal.Api.Models;

namespace Minimal.Api.Services
{
    public class BookService : IBookService
    {
        private readonly ApiDbContext dbContext;

        public BookService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<bool> CreateAsync(Book book)
        {
            var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Isbn == book.Isbn);
            if (existingBook != null)
            {
                return false;
            }
            await dbContext.AddAsync(book);
            var addBook =  await dbContext.SaveChangesAsync();
            return addBook > 0;
        }

        public async Task<bool?> DeleteAsync(string isbn)
        {
            var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Isbn == isbn);
            if (existingBook == null)
            {
                return false;
            }
            dbContext.Remove(existingBook);
            var result = await dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await dbContext.Books.ToListAsync();      
        }

        public async Task<Book?> GetByIsbnAsync(string Isbn)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Isbn == Isbn);
            if (book == null)
            {
                return null;
            }
            return book;
        }

        public async Task<bool?> UpdateAsync(string isbn, Book book)
        {
            var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Isbn.Equals(isbn));
            if (existingBook == null)
            {
                return false;
            }
            existingBook.Isbn = isbn;
            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Author = book.Author;
            existingBook.DateOfRelease = book.DateOfRelease;
            existingBook.PageCount = book.PageCount;
            var result = await dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
