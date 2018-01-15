using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class Referee 
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string name { get; set; }
        public virtual SportType SportType { get; set; }
        public int SportTypeID { get; set; }
        public virtual Country Country { get; set; }
        public int CountryID { get; set; }
    }
}
