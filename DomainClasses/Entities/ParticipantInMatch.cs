using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class ParticipantInMatch
    {        
        public int ID { get; set; }
        public int Participant_ID { get; set; }
        public Guid MatchID { get; set; }
        public Boolean IsHomeTeam { get; set; }
        //public double Score { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual Match Match { get; set; }        
    }
}
