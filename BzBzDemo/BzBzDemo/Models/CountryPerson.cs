using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BzBzDemo.Models
{
    public class CountryPerson
    {
        [ForeignKey(nameof(PersonId))]
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;
    }
}
