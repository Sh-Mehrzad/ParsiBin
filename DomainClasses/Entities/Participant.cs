using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class Participant : General.GeneralClassIntID
    {       
        public string Name { get; set; }
        public string Logo { get; set; }
        public int SportTypeID { get; set; }
        public int? ParticipantTypeID { get; set; }
        public int? CountryID { get; set; }
        public virtual SportType SportType { get; set; }
        public virtual  ICollection<Group> Groups { get; set; }        
        public virtual ICollection<ParticipantInMatch> PIMID { get; set; }
    }
}
