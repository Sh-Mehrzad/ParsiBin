using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses
{
    public class UserPrediction
    {
        public int ID { get; set; }
        public int PUserID { get; set; }
        public Guid MatchID { get; set; }
        public int ScoreTitleID { get; set; }
        public Double Score { get; set; }
        public Double Point { get; set; }
        public DateTime PredictionTime { get; set; }
        public int TournamentID { get; set; }
        public virtual User User { get; set; }
        public virtual Match Match { get; set; }
        public virtual ScoreTitle ScoreTitle { get; set; }
        public virtual MatchScore MatchScore { get; set; }        
    }
}
