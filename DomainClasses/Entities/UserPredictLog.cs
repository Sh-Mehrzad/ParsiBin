using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class UserPredictLog
    {
        public int ID { get; set; }
        public string PUserID { get; set; }
        public Guid MatchID { get; set; }
        public string ScoreTitleID { get; set; }
        public string Score { get; set; }        
        public DateTime PredictionTime { get; set; }
        public bool Status { get; set; }
    }
}
