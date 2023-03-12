namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
    using System.Globalization;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.WriteLine(GetBooksByAgeRestriction(db, "miNor"));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));

            //Console.WriteLine(GetBooksNotReleasedIn(db, 2000));

            //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));
            //Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, "e"));
            //Console.WriteLine(GetBookTitlesContaining(db, "WOR"));
            //Console.WriteLine(GetBooksByAuthor(db, "R"));
            //Console.WriteLine(CountBooks(db, 12));
            //Console.WriteLine(CountCopiesByAuthor(db));
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //Console.WriteLine(GetMostRecentBooks(db));

            //IncreasePrices(db);

            RemoveBooks(db);


        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

                string[] books = context.Books.Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

                return string.Join(Environment.NewLine, books).TrimEnd();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000).OrderBy(b => b.BookId).Select(b => b.Title).ToArray();

            return string.Join(Environment.NewLine, books).TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Price > 40)
                    .Select(b => new
                    {
                        Title = b.Title,
                        Price = b.Price
                    }).OrderByDescending(b => b.Price)
                    .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books.Where(b => b.ReleaseDate!.Value.Year != year)
                    .OrderBy(b => b.BookId)
                    .Select(b => b.Title)
                    .ToArray();

            return string.Join(Environment.NewLine, books).TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToLower()).ToArray();

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select (b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);        
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        { 
            var books = context.Books.Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(b => b.ReleaseDate)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books) 
            { 
            sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }
        
            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        { 
        var authors = context.Authors.Where(a => a.FirstName.EndsWith(input))
                .Select(a => new 
                { 
                FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var a in authors)
            {
                sb.AppendLine(a.FullName);
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        { 
        var books = context.Books.Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(a => a.Title)
                .Select(a => a.Title)
                .ToArray();
        
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books.Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                    .OrderBy(b => b.BookId)
                    .Select(b => new
                    {
                        b.Title,
                        b.Author.FirstName,
                        b.Author.LastName
                    })
                    .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        { 
        var books = context.Books.Where(b => b.Title.Length > lengthCheck).ToArray();

            return books.Length;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        { 
        var authorCopies = context.Authors.Select(a => new 
        { 
            AuthorName = a.FirstName + " " + a.LastName,
            AuthorBooksCopies = a.Books.Sum(b => b.Copies)
        }).OrderByDescending(a => a.AuthorBooksCopies)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authorCopies) 
            {
                sb.AppendLine($"{author.AuthorName} - {author.AuthorBooksCopies}");
            }
        
            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var bookCategoriesProfits = context.Categories.Select(c => new 
                { 
                CatName = c.Name,
                TotalalalalProfitMaina = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(c => c.TotalalalalProfitMaina).ThenBy(c => c.CatName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var bcp in bookCategoriesProfits)
            {
                sb.AppendLine($"{bcp.CatName} ${bcp.TotalalalalProfitMaina:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    CategoryRecentThreeBooks = c.CategoryBooks.OrderByDescending(cb => cb.Book.ReleaseDate).Take(3).Select(cb => new
                    {
                        BookTitle = cb.Book.Title,
                        BookReleaseDate = cb.Book.ReleaseDate!.Value.Year
                    }).ToArray()
                }).ToArray();


            StringBuilder sb = new StringBuilder();

            foreach (var category in categories) 
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.CategoryRecentThreeBooks)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.BookReleaseDate})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var booksBefore2010 = context.Books.Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010).ToArray();

            foreach (var book in booksBefore2010)
            {
                book.Price += 5;
            }
            
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Copies < 4200).ToArray();

            context.RemoveRange(books);
                        
            context.SaveChanges();
            
            return books.Count();
        }

    }
}


