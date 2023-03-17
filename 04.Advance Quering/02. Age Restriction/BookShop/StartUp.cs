namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.Write(GetMostRecentBooks(db));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            bool hasParsed = Enum.TryParse(typeof(AgeRestriction), command.ToLower(), true, out object? ageRestrictionObj);
            if (hasParsed)
            {
                AgeRestriction ageRestriction = (AgeRestriction)ageRestrictionObj;
                var sb = new StringBuilder();
                var titles = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .Select(b => b.Title)
                    .OrderBy(b => b);

                foreach (var t in titles)
                {
                    sb.AppendLine(t);
                }

                return sb.ToString().TrimEnd();
            }
            return null;
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.Copies < 5000
                && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, goldenBooks);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - ${b.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var titles = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var sb = new StringBuilder();
            string[] categories = input.Split().Select(c => c.ToLower()).ToArray();

            var titles = context.Books
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .Where(b => b.BookCategories
                                .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.Value < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(b => b.ReleaseDate.Value)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToArray();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - {b.EditionType.ToString()} - ${b.Price:F2}");
            }


            return sb.ToString().Trim();

        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorsNames = context.Authors
                .Where(a => EF.Functions.Like(a.FirstName, $"%{input}"))
                .Select(a => new { a.FirstName, a.LastName })
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToArray();

            return string.Join(Environment.NewLine, authorsNames.Select(an => $"{an.FirstName} {an.LastName}"));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var titles = context.Books
                .Where(b => EF.Functions.Like(b.Title.ToLower(), $"%{input.ToLower()}%"))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var titlesAndAuthors = context.Authors
                .Where(a => EF.Functions.Like(a.LastName.ToLower(), $"{input.ToLower()}%"))
                .SelectMany(a => a.Books.Select(b => new
                {
                    b.Title,
                    a.FirstName,
                    a.LastName,
                    b.BookId
                }))
                .OrderBy(b => b.BookId)
                .ToArray();

            return string.Join(Environment.NewLine, titlesAndAuthors.Select(ta => $"{ta.Title} ({ta.FirstName} {ta.LastName})"));

        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray();

            return books.Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorsCopies = context.Authors
                .Include(a => a.Books)
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToArray();

            return string.Join(Environment.NewLine, authorsCopies.Select(ac => $"{ac.FirstName} {ac.LastName} - {ac.TotalCopies}"));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesTotalProfit = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .OrderByDescending(ctp => ctp.TotalProfit)
                .ThenBy(ctp => ctp.Name)
                .ToArray();

            return string.Join(Environment.NewLine, categoriesTotalProfit.Select(ctp => $"{ctp.Name} ${ctp.TotalProfit:f2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Take(3)
                    .Select(cb => new
                    {
                        cb.Book.Title,
                        cb.Book.ReleaseDate
                    })
                })
                .ToArray();

            foreach (var c in categories)
            {
                sb.AppendLine($"--{c.CategoryName}");
                foreach(var b in c.Books)
                {
                    sb.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var b in books)
            {
                b.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
                
            int startCount = context.Books.Count();

            var books = context.Books
                .Where(b => b.Copies < 4200); ;

            foreach(var b in books)
            {
                context.Books.Remove(b);
            }
            context.SaveChanges();
            int EndCount = context.Books.Count();

            return startCount - EndCount;
        }
    }

}


