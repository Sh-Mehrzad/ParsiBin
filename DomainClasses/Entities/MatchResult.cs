using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class MatchResult
    {
        public int ID { get; set; }
        public Guid MatchID { get; set; }
        public int HomeGoal { get; set; }
        public int AwayGoal { get; set; }
    }
}
