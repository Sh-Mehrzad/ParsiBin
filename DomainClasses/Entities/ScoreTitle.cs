using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class ScoreTitle
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<MatchScore> MatchScoreList { get; set; }
    }
}
