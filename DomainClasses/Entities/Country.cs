using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class Country
    {
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Flag { get; set; }
        public virtual ICollection<City> CityList { get; set; }
        public virtual ICollection<Referee> RefereeList { get; set; }
    }
}
