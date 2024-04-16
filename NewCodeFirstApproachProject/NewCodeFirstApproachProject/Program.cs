using NewCodeFirstApproachProject.Models;

namespace NewCodeFirstApproachProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            var context = new NCfContext();

            ResetDatabase(context, true);
        }

        private static void ResetDatabase(NCfContext context, bool toBeDeleted = false)
        {
            if (toBeDeleted)
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
