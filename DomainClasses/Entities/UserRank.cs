using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.Entities
{
    public class UserRank
    {
        public int ID { get; set; }
        public int Rank { get; set; }
        public int UserID { get; set; }
        public double UserPoint { get; set; }
        public virtual User User { get; set; }
        public DateTime RankDate { get; set; }
        public int TournamentID { get; set; }

    }
}
