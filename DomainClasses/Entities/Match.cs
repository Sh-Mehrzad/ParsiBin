using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class Match : General.GeneralClass<Guid>
    {
        public DateTime MatchTime { get; set; }
        public byte MatchStatus { get; set; }        
        public int StadiumID { get; set; }
        public virtual Referee Referee { get; set; }
        public int RefereeID { get; set; }
        public virtual ICollection<ParticipantInMatch> PIMID { get; set; }
        public virtual Group Group { get; set; }
        public Guid GroupID { get; set; }
        public virtual MatchType MatchType { get; set; }
        public virtual ICollection<MatchScore> MatchScore { get; set; }
    }
}
