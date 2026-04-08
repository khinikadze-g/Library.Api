using System.ComponentModel.DataAnnotations;

namespace Minimal.Api.Models
{
    public class Book
    {
        [Key]
        public string Isbn { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? Description { get; set; }
        public int PageCount { get; set; }
        public DateTime DateOfRelease { get; set; }


    }
}
