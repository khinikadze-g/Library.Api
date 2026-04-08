using Microsoft.EntityFrameworkCore;
using Minimal.Api.Models;

namespace Minimal.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> contextOptions) : base (contextOptions)
        { 
        }

        public DbSet<Book> Books { get; set;}
    }
}
