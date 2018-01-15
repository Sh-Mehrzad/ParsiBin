using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses
{
    public class SportType : General.GeneralClassIntID
    {
        [MaxLength(50)]
        public string Title { get; set; }        
        public virtual ICollection<Participant> Participant { get; set; }
    }
}
