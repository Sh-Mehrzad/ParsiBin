using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class ParticipantInGroups : General.GeneralClassIntID
    {        
        public Guid GroupID { get; set; }
        public int ParticipantID { get; set; }
        public virtual Participant participant { get; set; }
    }
}
