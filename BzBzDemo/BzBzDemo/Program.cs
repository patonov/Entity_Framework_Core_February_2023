using BzBzDemo.Models;

namespace BzBzDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new BzContext();

            ResetDatabase(context, toDropDb: true);
        }

        private static void ResetDatabase(BzContext context, bool toDropDb = false)
        {
            if (toDropDb)
            { 
            context.Database.EnsureDeleted();
            }

            if (context.Database.EnsureCreated())
            { 
            return;
            }

        }
    }
}
