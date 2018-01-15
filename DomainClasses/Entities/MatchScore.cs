using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class MatchScore : General.GeneralClassIntID
    {
        //public Int64 PIGID { get; set; }
        //public virtual ParticipantInMatch ParticipantInMatch  { get; set; }
        public Guid MatchID { get; set; }
        public virtual Match Match { get; set; }
        public virtual ScoreTitle ScoreTitle { get; set; }
        public int ScoreTitleID { get; set; }
        public double Score { get; set; }

    }
}
