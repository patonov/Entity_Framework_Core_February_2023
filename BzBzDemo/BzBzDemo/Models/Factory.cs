using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BzBzDemo.Models
{
    public class Factory
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!; 

        public ICollection<Person> People { get; set; } = new List<Person>();
    }
}
