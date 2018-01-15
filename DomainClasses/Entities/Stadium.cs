using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class Stadium : General.GeneralClassIntID
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public int CityID { get; set; }
        public virtual City City { get; set; }
    }
}
