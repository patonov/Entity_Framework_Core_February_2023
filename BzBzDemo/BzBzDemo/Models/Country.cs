using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BzBzDemo.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public virtual ICollection<CountryPerson> CountryPeople { get; set; } = new List<CountryPerson>();
    }
}
