namespace PureLinqQueries
{
    using System;
    using System.Linq;

    internal class Program
    {
        static void Main(string[] args)
        {
            int[] nums = [11, 12, 13, 14, 15];

            IEnumerable<int> selectedNums =
                from num in nums
                where num > 13
                select num;

            foreach (var n in selectedNums)
            {
                Console.WriteLine(n);
            }
        }
    }
}
