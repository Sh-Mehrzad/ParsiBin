using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class City
    {
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public virtual Country Country { get; set; }
        public int CountryID { get; set; }
        public virtual ICollection<Stadium> StadiumList { get; set; }
    }
}
