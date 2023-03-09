namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
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

            Console.WriteLine(GetBooksNotReleasedIn(db, 2000));
        
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

    }
}


