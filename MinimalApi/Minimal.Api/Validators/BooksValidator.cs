using FluentValidation;
using Minimal.Api.Models;

namespace Minimal.Api.Validators
{
    public class BooksValidator : AbstractValidator<Book>
    {
        public BooksValidator()
        {
            RuleFor(Book => Book.Isbn).
                Matches(@"^(?:(?:\D*\d){10}(?:(?:\D*\d){3})?)\D*$")
                .WithMessage("ISBN must be either 10 or 13 digits, with optional separators");
            RuleFor(Book => Book.Title).NotEmpty();
            RuleFor(Book => Book.Author).NotEmpty();
            RuleFor(Book => Book.DateOfRelease).LessThanOrEqualTo(DateTime.UtcNow)
                                .WithMessage("Release year cannot be in the future.");
            RuleFor(Book => Book.PageCount).NotEmpty();

        }
    }
}
