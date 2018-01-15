using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class PartInMatchModel
    {
        public int ID { get; set; }
        public int Participant_ID { get; set; }
        public string ParticipantName { get; set; }
        public Guid MatchID { get; set; }
        public Boolean IsHomeTeam { get; set; }
        //public double Score { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual Match Match { get; set; }
    }
}
