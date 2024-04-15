using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BzBzDemo.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Names { get; set; } = null!;

        public string Profession { get; set; } = null!;
        
        [ForeignKey(nameof(FactoryId))]
        public int FactoryId { get; set; }
        public Factory Factory { get; set; } = null!;

        public virtual ICollection<CountryPerson> PeopleCountry { get; set; } = new List<CountryPerson>();
    }
}
