namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
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
            Console.WriteLine(GetBookTitlesContaining(db, "WOR"));
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



    }
}


